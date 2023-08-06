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
    private readonly List<int> _availableRegions;
    
    public InstallAndUpdateFIASJob(
        IServiceProvider provider,
        ILogger<ActualizeFIASVersionHistoryJob> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;
        
        _availableRegions = _configuration
            .GetSection("FIASToolSet:Regions")
            .Get<List<string>>()
            .DefaultIfEmpty()
            .Where(e => int.TryParse(e, out _))
            .Select(int.Parse)
            .ToList();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
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
                }

                await loader.SetInstallationToStatusInstalled();
            }
            else
            {
                _logger.LogInformation("Не обнаружены версии ФИАС для установки или обновления. Действие пропущено.");
            }
        }

        _logger.LogInformation("Завершена обработка шагов по установке / обновлению классификатора ФИАС.");
    }
}