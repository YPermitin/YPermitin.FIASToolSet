using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.DistributionLoader;
using YPermitin.FIASToolSet.DistributionLoader.Exceptions;
using YPermitin.FIASToolSet.DistributionLoader.Models;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;

namespace YPermitin.FIASToolSet.Jobs.JobItems;

/// <summary>
/// Задание для установки и обновления классификатора ФИАС
/// </summary>
[DisallowConcurrentExecution]
public class InstallAndUpdateFIASJob : IJob
{
    private readonly ILogger<InstallAndUpdateFIASJob> _logger;
    private readonly IServiceProvider _provider;
    private readonly IConfiguration _configuration;

    private readonly string _workingDirectory;
    private readonly List<int> _availableRegions;
    private readonly bool _removeArchiveDistributionFiles;
    private readonly bool _removeExtractedDistributionFiles;
    private readonly int _maxParallelTasks;
    
    public InstallAndUpdateFIASJob(
        IServiceProvider provider,
        ILogger<InstallAndUpdateFIASJob> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;

        _workingDirectory = _configuration.GetValue("FIASToolSet:WorkingDirectory", string.Empty);

        var availableRegionsAsStrings = _configuration
            .GetSection("FIASToolSet:Regions")
            .Get<List<string>>();
        if (availableRegionsAsStrings == null || availableRegionsAsStrings.Count == 0)
        {
            _availableRegions = new List<int>();
        }
        else
        {
            _availableRegions = availableRegionsAsStrings
                .DefaultIfEmpty()
                .Where(e => int.TryParse(e, out _))
                .Select(int.Parse)
                .ToList();
        }

        _removeArchiveDistributionFiles =
            _configuration.GetValue("FIASToolSet:ClearTempDistributionFiles:RemoveArchiveDistributionFiles", false);
        _removeExtractedDistributionFiles =
            _configuration.GetValue("FIASToolSet:ClearTempDistributionFiles:RemoveExtractedDistributionFiles", true);
        _maxParallelTasks =
            _configuration.GetValue("FIASToolSet:MaxParallelTasks", 1);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            ClearTempDistributionFiles();
            
            await ExecuteInstallOrUpdateFIAS();
        }
        catch (Exception e)
        {
            _logger.LogError(e, 
                "Ошибка при выполнении задания по установке / обновлению классификатора ФИАС.");
        }
    }

    private async Task ExecuteInstallOrUpdateFIAS()
    {
        // TODO Привести в порядок кодовую базу задания, вынести в отдельные классы функционал,
        // а также избавиться от дублирования кода.
        _logger.LogInformation("Запущена обработка шагов по установке / обновлению классификатора ФИАС.");

        Guid installationVersionId = Guid.Empty;
        List<Region> availableRegions = null;
        
        // Этап №1: Подготовка задания загрузки данных ФИАС и обновление статусов задания.
        // Распаковка архива ФИАС и подготовка файлов дистрибутива. Загрузка базовых справочников ФИАС.
        using (var scope = _provider.CreateScope())
        {
            IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
            
            await loader.FixStuckInstallationExists();
            
            if (await loader.ActiveInstallationExists())
            {
                _logger.LogWarning(
                    "Обнаружена активная установка (процесс установки или обновления) классификатора ФИАС. Задание прервано.");
                return;
            }

            if (await loader.InitVersionInstallationToLoad())
            {
                await loader.SetInstallationToStatusInstalling();
                if (loader.VersionInstallation.InstallationTypeId == FIASVersionInstallationType.Full)
                {
                    if (string.IsNullOrEmpty(loader.VersionInstallation.FIASVersion.GARFIASXmlComplete))
                    {
                        _logger.LogWarning(
                            $"Версия ФИАС с типом FULL не содержит ссылки на загрузку полной версии дистрибутива ФИАС. Установка пропущена.");
                        await loader.SetInstallationToStatusInstalled();
                        return;
                    }
                }
                else if (loader.VersionInstallation.InstallationTypeId == FIASVersionInstallationType.Update)
                {
                    if (string.IsNullOrEmpty(loader.VersionInstallation.FIASVersion.GARFIASXmlDelta))
                    {
                        _logger.LogWarning(
                            $"Версия ФИАС с типом DELTA не содержит ссылки на загрузку обновлений версии дистрибутива ФИАС. Установка пропущена.");
                        await loader.SetInstallationToStatusInstalled();
                        return;
                    }
                }
                
                bool downloadSuccess = true;
                DateTime downloadStarted = DateTime.UtcNow;
                DateTime lastProgressShow = DateTime.MinValue;
                double lastProgressPercentage = 0;

                await loader.DownloadAndExtractDistribution(args =>
                {
                    double progress = Math.Round(args.ProgressPercentage, 2);
                    double progressChanged = lastProgressPercentage - progress;

                    var showProgressTimeLeft = DateTime.UtcNow - lastProgressShow;
                    if (args.State != DownloadDistributionFileProgressChangedEventType.Downloading
                        || showProgressTimeLeft.TotalSeconds > 10
                        || progressChanged >= 1)
                    {
                        lastProgressPercentage = progress;
                        lastProgressShow = DateTime.UtcNow;

                        var downloadDuration = DateTime.UtcNow - downloadStarted;
                        _logger.LogInformation(
                            $"{args.State}. Загружено: {progress} %. " +
                            $"Прошло секунд: {Math.Round(downloadDuration.TotalSeconds, 2)}");
                        if (args.ErrorInfo != null)
                        {
                            _logger.LogError(args.ErrorInfo, "Ошибка при загрузке файла дистрибутива ФИАС.");
                            downloadSuccess = false;
                        }
                    }
                });

                if (!downloadSuccess)
                {
                    await loader.SetInstallationToStatusNew();
                    return;
                }

                await loader.LoadAllBaseCatalogs();
                
                availableRegions = loader.GetAvailableRegions();
                if (_availableRegions.Count > 0)
                {
                    availableRegions = availableRegions
                        .Where(e => _availableRegions.Contains(e.Code))
                        .ToList();
                }

                installationVersionId = loader.VersionInstallation.Id;
            }
            else
            {
                _logger.LogInformation("Не обнаружены версии ФИАС для установки или обновления. Действие пропущено.");
                return;
            }
        }
        
        // Этап №2: Формирование заданий по загрузке данных в разрезе регионов.
        // Может использоваться многопоточная загрузка, если разрешена.
        CancellationTokenSource cancelTokenSourceForLoadOperations = new CancellationTokenSource(); 
        CancellationToken cancellationToken = cancelTokenSourceForLoadOperations.Token;
        if (availableRegions != null)
        {
            foreach (var availableRegion in availableRegions)
            {
                using (var scope = _provider.CreateScope())
                {
                    IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                    bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                    if (!installationConfigured)
                    {
                        _logger.LogInformation("Не удалось настроить версии ФИАС для установки или обновления. Действие пропущено.");
                        return;
                    }
                    await loader.DownloadAndExtractDistribution(null, true);
                    
                    bool regionWasLoaded = await loader.RegionWasLoaded(availableRegion.Code);
                    if (regionWasLoaded)
                    {
                        _logger.LogWarning(
                            $"Регион с кодом \"{availableRegion.Code}\" уже был загружен. Пропускаем обработки файлов данных.");
                        continue;
                    }

                    try
                    {
                        loader.ExtractDataForRegion(availableRegion);
                    }
                    catch (RegionNotFoundException)
                    {
                        _logger.LogError(
                            $"Не найден регион с кодом ${availableRegion.Code} среди доступных регионов в дистрибутиве ФИАС. Загрузка пропущена.");
                        continue;
                    }

                    await loader.SetRegionInstallationStatusToInstalling(availableRegion.Code);
                }
                
                List<Task> tasksIsRunning = new List<Task>();
                ConcurrentQueue<Func<Task>> tasksToRun = new ConcurrentQueue<Func<Task>>();

                #region Tasks
                Func<Task> taskLoadNormativeDocuments = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        
                        await loader.LoadNormativeDocuments(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadNormativeDocuments);
                Func<Task> taskLoadAddressObjects = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadAddressObjects(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadAddressObjects);
                Func<Task> taskLoadAddressObjectDivisions = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadAddressObjectDivisions(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadAddressObjectDivisions);
                Func<Task> taskLoadAddressObjectsAdmHierarchy = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadAddressObjectsAdmHierarchy(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadAddressObjectsAdmHierarchy);
                Func<Task> taskLoadAddressObjectsMunHierarchy = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadAddressObjectsMunHierarchy(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadAddressObjectsMunHierarchy);
                Func<Task> taskLoadApartments = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadApartments(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadApartments);
                Func<Task> taskLoadCarPlaces = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadCarPlaces(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadCarPlaces);
                Func<Task> taskLoadHouses = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadHouses(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadHouses);
                Func<Task> taskLoadRooms = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadRooms(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadRooms);
                Func<Task> taskLoadSteads = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadSteads(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadSteads);
                Func<Task> taskLoadAddressObjectParameters = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadAddressObjectParameters(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadAddressObjectParameters);
                Func<Task> taskLoadApartmentParameters = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadApartmentParameters(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadApartmentParameters);
                Func<Task> taskLoadCarPlaceParameters = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadCarPlaceParameters(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadCarPlaceParameters);
                Func<Task> taskLoadHouseParameters = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadHouseParameters(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadHouseParameters);
                Func<Task> taskLoadRoomParameters = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadRoomParameters(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadRoomParameters);
                Func<Task> taskLoadSteadParameters = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadSteadParameters(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadSteadParameters);
                Func<Task> taskLoadChangeHistory = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadChangeHistory(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadChangeHistory);
                Func<Task> taskLoadObjectsRegistry = async () =>
                {
                    using (var scope = _provider.CreateScope())
                    {
                        IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                        bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                        if (!installationConfigured)
                        {
                            _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                            return;
                        }
                        await loader.DownloadAndExtractDistribution(null, true);
                        loader.ExtractDataForRegion(availableRegion, true);
                        await loader.LoadObjectsRegistry(availableRegion, cancellationToken);
                    }
                };
                tasksToRun.Enqueue(taskLoadObjectsRegistry);
                #endregion

                while (tasksToRun.Count > 0 || tasksIsRunning.Count > 0)
                {
                    // Запускаем задачи для заполнения пула активных задач
                    while (tasksIsRunning.Count < _maxParallelTasks && tasksToRun.Count > 0)
                    {
                        try
                        {
                            if (tasksToRun.TryDequeue(out Func<Task> taskItemToRun))
                            {
                                var startedTask = taskItemToRun();
                                tasksIsRunning.Add(startedTask);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    // Ждем завершения хотя бы одной задачи
                    Task.WaitAny(tasksIsRunning.ToArray());

                    foreach (var taskRunning in tasksIsRunning)
                    {
                        if (taskRunning.IsFaulted)
                        {
                            cancelTokenSourceForLoadOperations.Cancel();
                            _logger.LogError(taskRunning.Exception, $"Ошибка при загрузке данных ФИАС.");

                            try
                            {
                                Task.WaitAll(tasksIsRunning.ToArray());
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, 
                                    $"Отменены операции загрузки данных из-за ошибки в одном из заданий. Подробности см. выше.");
                            }
                            finally
                            {
                                cancelTokenSourceForLoadOperations.Dispose();
                            }

                            throw new Exception("Ошибка при загрузке данных ФИАС.", taskRunning.Exception);
                        }
                    }

                    // Удаляем выполненные задачи из пула
                    tasksIsRunning = tasksIsRunning
                        .Where(e => !e.IsCompleted)
                        .ToList();
                }

                using (var scope = _provider.CreateScope())
                {
                    IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
                    bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
                    if (!installationConfigured)
                    {
                        _logger.LogInformation($"Не удалось настроить версии ФИАС для установки или обновления по региону {availableRegion.Code}. Действие пропущено.");
                        return;
                    }
                    await loader.DownloadAndExtractDistribution(null, true);
                    loader.ExtractDataForRegion(availableRegion, true);
                    
                    await loader.SetRegionInstallationStatusToInstalled(availableRegion.Code);

                    if (_removeExtractedDistributionFiles)
                    {
                        loader.RemoveDistributionRegionDirectory(availableRegion);
                    }
                }
            }
        }

        // Этап №3: Установка финального статуса для задания установки / обновления ФИАС.
        // Очистка временных файлов в соответствии с настройками.
        using (var scope = _provider.CreateScope())
        {
            IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();
            bool installationConfigured = await loader.SetVersionInstallationToLoad(installationVersionId);
            if (!installationConfigured)
            {
                _logger.LogInformation("Не удалось настроить версии ФИАС для установки или обновления. Действие пропущено.");
                return;
            }
            await loader.DownloadAndExtractDistribution(null, true);
            
            await loader.SetInstallationToStatusInstalled();
            if (_removeExtractedDistributionFiles)
            {
                loader.RemoveVersionDataDirectory();
            }

            if (_removeArchiveDistributionFiles)
            {
                loader.RemoveVersionDataArchive();
            }
        }

        _logger.LogInformation("Завершена обработка шагов по установке / обновлению классификатора ФИАС.");
    }

    private void ClearTempDistributionFiles()
    {
        if (!_removeArchiveDistributionFiles && !_removeExtractedDistributionFiles)
            return;

        _logger.LogInformation("Начало очистки временных файлов...");
        
        var versionDirectories = Directory.GetDirectories(_workingDirectory, 
            "*.*", 
            SearchOption.TopDirectoryOnly);
        foreach (var versionDirectory in versionDirectories)
        {
            var versionDirectoryInfo = new DirectoryInfo(versionDirectory);
            if (versionDirectoryInfo.Exists)
            {
                if (_removeArchiveDistributionFiles)
                {
                    var archiveFiles = Directory.GetFiles(versionDirectory, "*.zip");
                    foreach (var archiveFile in archiveFiles)
                    {
                        File.Delete(archiveFile);
                        _logger.LogInformation($"Удален архив дистрибутива: {archiveFile}.");
                    }
                }

                if (_removeExtractedDistributionFiles)
                {
                    var distributionDirectories =
                        Directory.GetDirectories(versionDirectory, 
                            "*", 
                            SearchOption.TopDirectoryOnly);
                    foreach (var distributionDirectory in distributionDirectories)
                    {
                        Directory.Delete(distributionDirectory, true);
                        _logger.LogInformation($"Удален распакованный каталог дистрибутива: {distributionDirectory}.");
                    }
                }

                var filesInVersionDirectory = Directory.GetFiles(versionDirectory, "*.*");
                var directoriesInVersionDirectory = Directory.GetDirectories(versionDirectory);
                if (filesInVersionDirectory.Length == 0 && directoriesInVersionDirectory.Length == 0)
                {
                    versionDirectoryInfo.Delete(true);
                    _logger.LogInformation($"Удален каталог версии дистрибутива: {versionDirectoryInfo.FullName}.");
                }
            }
        }
        
        _logger.LogInformation("Завершение очистки временных файлов...");
    }
}