using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionLoader;
using YPermitin.FIASToolSet.DistributionReader;
using YPermitin.FIASToolSet.Storage.Core.Models;
using YPermitin.FIASToolSet.Storage.Core.Services;

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
    
    public InstallAndUpdateFIASJob(
        IServiceProvider provider,
        ILogger<ActualizeFIASVersionHistoryJob> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Запущена обработка шагов по установке / обновлению классификатора ФИАС.");

        using (var scope = _provider.CreateScope())
        {
            IFIASDistributionLoader loader = scope.ServiceProvider.GetRequiredService<IFIASDistributionLoader>();

            if (await loader.ActiveInstallationExists())
            {
                _logger.LogWarning(
                    ".Обнаружена активная установка (процесс установки или обновления) классификатора ФИАС. Задание прервано.");
                return;
            }

            if (await loader.InitVersionInstallationToLoad())
            {
                await loader.SetInstallationToStatusInstalling();

                await loader.DownloadAndExtractDistribution();

                await loader.LoadApartmentTypes();
                await loader.LoadHouseTypes();
                await loader.LoadObjectLevels();
                await loader.LoadOperationTypes();
                await loader.LoadRoomTypes();
                await loader.LoadParameterTypes();
                await loader.LoadAddressObjectTypes();
                await loader.LoadNormativeDocKinds();
                await loader.LoadNormativeDocTypes();

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