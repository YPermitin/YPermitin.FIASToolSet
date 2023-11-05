using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.DistributionLoader;

namespace YPermitin.FIASToolSet.Jobs.JobItems;

/// <summary>
/// Задание для установки и обновления классификатора ФИАС
/// </summary>
[DisallowConcurrentExecution]
public class InstallAndUpdateFIASJob : IJob
{
    private readonly ILogger<ActualizeFIASVersionHistoryJob> _logger;
    private readonly IServiceProvider _provider;
    private readonly IConfiguration _configuration;

    private readonly string _workingDirectory;
    private readonly List<int> _availableRegions;
    private readonly bool _removeArchiveDistributionFiles;
    private readonly bool _removeExtractedDistributionFiles;
    
    public InstallAndUpdateFIASJob(
        IServiceProvider provider,
        ILogger<ActualizeFIASVersionHistoryJob> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;

        _workingDirectory = _configuration.GetValue("FIASToolSet:WorkingDirectory", string.Empty);
        
        _availableRegions = _configuration
            .GetSection("FIASToolSet:Regions")
            .Get<List<string>>()
            .DefaultIfEmpty()
            .Where(e => int.TryParse(e, out _))
            .Select(int.Parse)
            .ToList();

        _removeArchiveDistributionFiles =
            _configuration.GetValue("FIASToolSet:ClearTempDistributionFiles:RemoveArchiveDistributionFiles", false);
        _removeExtractedDistributionFiles =
            _configuration.GetValue("FIASToolSet:ClearTempDistributionFiles:RemoveExtractedDistributionFiles", true);
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
        _logger.LogInformation("Запущена обработка шагов по установке / обновлению классификатора ФИАС.");

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

                await loader.LoadApartmentTypes();
                await loader.LoadHouseTypes();
                await loader.LoadObjectLevels();
                await loader.LoadOperationTypes();
                await loader.LoadRoomTypes();
                await loader.LoadParameterTypes();
                await loader.LoadAddressObjectTypes();
                await loader.LoadNormativeDocKinds();
                await loader.LoadNormativeDocTypes();
                
                var availableRegions = loader.GetAvailableRegions()
                    .Where(e => _availableRegions.Contains(e.Code));
                foreach (var availableRegion in availableRegions)
                {
                    loader.ExtractDataForRegion(availableRegion);
                    
                    await loader.LoadAddressObjects(availableRegion);
                    await loader.LoadAddressObjectsAdmHierarchy(availableRegion);
                    await loader.LoadAddressObjectDivisions(availableRegion);
                    await loader.LoadAddressObjectParameters(availableRegion);
                    await loader.LoadApartments(availableRegion);
                    await loader.LoadApartmentParameters(availableRegion);
                    await loader.LoadCarPlaces(availableRegion);
                    await loader.LoadCarPlaceParameters(availableRegion);
                    await loader.LoadHouses(availableRegion);
                    await loader.LoadHouseParameters(availableRegion);
                    await loader.LoadRooms(availableRegion);
                    await loader.LoadRoomParameters(availableRegion);
                    await loader.LoadSteads(availableRegion);
                    await loader.LoadSteadParameters(availableRegion);

                    if (_removeExtractedDistributionFiles)
                    {
                        loader.RemoveDistributionRegionDirectory(availableRegion);
                    }
                }

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
            else
            {
                _logger.LogInformation("Не обнаружены версии ФИАС для установки или обновления. Действие пропущено.");
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