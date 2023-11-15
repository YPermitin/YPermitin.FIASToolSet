using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Extensions;
using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.Storage.Core.Models.Notifications;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.Jobs.JobItems
{
    /// <summary>
    /// Задание актуализации информации о версиях ФИАС
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class ActualizeFIASVersionHistoryJob : IJob
    {
        private readonly ILogger<ActualizeFIASVersionHistoryJob> _logger;
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        public ActualizeFIASVersionHistoryJob(
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
            _logger.LogInformation("Запущена акуализация информации о версиях ФИАС.");

            using (var scope = _provider.CreateScope())
            {
                IFIASDistributionBrowser fiasDistributionBrowser = scope.ServiceProvider.GetRequiredService<IFIASDistributionBrowser>();
                IFIASMaintenanceRepository fiasMaintenanceService = scope.ServiceProvider.GetRequiredService<IFIASMaintenanceRepository>();
                IFIASInstallationManagerRepository fiasInstallationManagerService = scope.ServiceProvider.GetRequiredService<IFIASInstallationManagerRepository>();

                // Последняя версия из хранилища
                var lastVersion = await fiasMaintenanceService.GetLastVersion();
                
                // Получаем историю версий ФИАС, чтобы дозагрузить все пропущенные версии, а не только последнюю.
                IReadOnlyList<FIASDistributionInfo> fiasVersionHistory;
                if (lastVersion == null)
                {
                    fiasVersionHistory = new List<FIASDistributionInfo>()
                    {
                        await fiasDistributionBrowser.GetLastDistributionInfo()
                    }.AsReadOnly();
                }
                else
                {
                    fiasVersionHistory = await fiasDistributionBrowser.GetAllDistributionInfo();
                    fiasVersionHistory = fiasVersionHistory
                        .Where(e => e.VersionId >= lastVersion.VersionId && e.Date >= lastVersion.Date)
                        .OrderBy(e => e.VersionId)
                        .ToList()
                        .AsReadOnly();
                }
                
                foreach (var fiasActualDistributionInfo in fiasVersionHistory)
                {
                    _logger.LogInformation($"Начало проверки версии ФИАС: {fiasActualDistributionInfo.VersionId}.");
                    
                    bool versionChanged;
                    // Последняя версия из хранилища
                    lastVersion = await fiasMaintenanceService.GetLastVersion();
                    if (lastVersion == null)
                    {
                        _logger.LogInformation("Информацию о предыдущей версии ФИАС в хранилище не обнаружено.");
                        versionChanged = true;
                    }
                    else
                    {
                        // Если версии различаются хотя бы одним полем, то добавляем новую версию
                        versionChanged =
                            lastVersion.VersionId != fiasActualDistributionInfo.VersionId
                            || lastVersion.Date != fiasActualDistributionInfo.Date
                            || lastVersion.TextVersion != fiasActualDistributionInfo.TextVersion
                            || lastVersion.FIASDbfComplete !=
                            fiasActualDistributionInfo.FIASDbf.Complete.GetAbsoluteUri()
                            || lastVersion.FIASDbfDelta != fiasActualDistributionInfo.FIASDbf.Delta.GetAbsoluteUri()
                            || lastVersion.FIASXmlComplete !=
                            fiasActualDistributionInfo.FIASXml.Complete.GetAbsoluteUri()
                            || lastVersion.FIASXmlDelta != fiasActualDistributionInfo.FIASXml.Delta.GetAbsoluteUri()
                            || lastVersion.GARFIASXmlComplete !=
                            fiasActualDistributionInfo.GARFIASXml.Complete.GetAbsoluteUri()
                            || lastVersion.GARFIASXmlDelta !=
                            fiasActualDistributionInfo.GARFIASXml.Delta.GetAbsoluteUri()
                            || lastVersion.KLADR4ArjComplete !=
                            fiasActualDistributionInfo.KLADR4Arj.Complete.GetAbsoluteUri()
                            || lastVersion.KLADR47zComplete !=
                            fiasActualDistributionInfo.KLADR47z.Complete.GetAbsoluteUri();

                        if (versionChanged)
                        {
                            _logger.LogInformation("Зафиксированы изменения в данных версии ФИАС.");
                            _logger.LogInformation($"Period: [{lastVersion.Period}] -> [-].");
                            _logger.LogInformation($"Id: [{lastVersion.Id}] -> [-].");
                            _logger.LogInformation(
                                $"VersionId: [{lastVersion.VersionId}] -> [{fiasActualDistributionInfo.VersionId}].");
                            _logger.LogInformation(
                                $"TextVersion: [{lastVersion.TextVersion}] -> [{fiasActualDistributionInfo.TextVersion}].");
                            _logger.LogInformation(
                                $"Date: [{lastVersion.Date}] -> [{fiasActualDistributionInfo.Date}].");
                            _logger.LogInformation(
                                $"FIASDbfComplete: [{lastVersion.FIASDbfComplete}] -> [{fiasActualDistributionInfo.FIASDbf.Complete}].");
                            _logger.LogInformation(
                                $"FIASDbfDelta: [{lastVersion.FIASDbfDelta}] -> [{fiasActualDistributionInfo.FIASDbf.Delta}].");
                            _logger.LogInformation(
                                $"FIASXmlComplete: [{lastVersion.FIASXmlComplete}] -> [{fiasActualDistributionInfo.FIASXml.Complete}].");
                            _logger.LogInformation(
                                $"FIASXmlDelta: [{lastVersion.FIASXmlDelta}] -> [{fiasActualDistributionInfo.FIASXml.Delta}].");
                            _logger.LogInformation(
                                $"GARFIASXmlComplete: [{lastVersion.GARFIASXmlComplete}] -> [{fiasActualDistributionInfo.GARFIASXml.Complete}].");
                            _logger.LogInformation(
                                $"GARFIASXmlDelta: [{lastVersion.GARFIASXmlDelta}] -> [{fiasActualDistributionInfo.GARFIASXml.Delta}].");
                            _logger.LogInformation(
                                $"KLADR4ArjComplete: [{lastVersion.KLADR4ArjComplete}] -> [{fiasActualDistributionInfo.KLADR4Arj.Complete}].");
                            _logger.LogInformation(
                                $"KLADR47zComplete: [{lastVersion.KLADR47zComplete}] -> [{fiasActualDistributionInfo.KLADR47z.Complete}].");
                        }
                    }

                    if (versionChanged)
                    {
                        await fiasMaintenanceService.BeginTransactionAsync();

                        var newFIASVersion = new FIASVersion()
                        {
                            Period = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                            TextVersion = fiasActualDistributionInfo.TextVersion,
                            Date = fiasActualDistributionInfo.Date,
                            VersionId = fiasActualDistributionInfo.VersionId,
                            FIASDbfComplete = fiasActualDistributionInfo.FIASDbf.Complete.GetAbsoluteUri(),
                            FIASDbfDelta = fiasActualDistributionInfo.FIASDbf.Delta.GetAbsoluteUri(),
                            FIASXmlComplete = fiasActualDistributionInfo.FIASXml.Complete.GetAbsoluteUri(),
                            FIASXmlDelta = fiasActualDistributionInfo.FIASXml.Delta.GetAbsoluteUri(),
                            GARFIASXmlComplete = fiasActualDistributionInfo.GARFIASXml.Complete.GetAbsoluteUri(),
                            GARFIASXmlDelta = fiasActualDistributionInfo.GARFIASXml.Delta.GetAbsoluteUri(),
                            KLADR4ArjComplete = fiasActualDistributionInfo.KLADR4Arj.Complete.GetAbsoluteUri(),
                            KLADR47zComplete = fiasActualDistributionInfo.KLADR47z.Complete.GetAbsoluteUri()
                        };

                        await fiasMaintenanceService.AddVersion(newFIASVersion);

                        bool useNotifications = _configuration.GetValue("Jobs:EnableNotification", false);
                        if (useNotifications)
                        {
                            await fiasMaintenanceService.AddNotification(new NotificationQueue()
                            {
                                Period = DateTime.UtcNow,
                                Id = Guid.NewGuid(),
                                NotificationTypeId = NotificationType.NewVersionOfFIAS,
                                StatusId = NotificationStatus.Added,
                                FIASVersionId = newFIASVersion.Id,
                                Content = null
                            });
                        }

                        await fiasMaintenanceService.SaveAsync();

                        bool fullUrlChanged;
                        bool deltaUrlChanged;
                        if (lastVersion == null)
                        {
                            fullUrlChanged = true;
                            deltaUrlChanged = true;
                        }
                        else
                        {
                            fullUrlChanged = lastVersion.GARFIASXmlComplete != newFIASVersion.GARFIASXmlComplete;
                            deltaUrlChanged = lastVersion.GARFIASXmlDelta != newFIASVersion.GARFIASXmlDelta;
                        }

                        if (fullUrlChanged || deltaUrlChanged)
                        {
                            var lastInstallation = await fiasInstallationManagerService.GetLastInstallation();
                            if (lastInstallation == null)
                            {
                                if (fullUrlChanged)
                                {
                                    fiasInstallationManagerService.AddInstallation(new FIASVersionInstallation()
                                    {
                                        Id = Guid.NewGuid(),
                                        FIASVersionId = newFIASVersion.Id,
                                        StatusId = FIASVersionInstallationStatus.New,
                                        InstallationTypeId = FIASVersionInstallationType.Full,
                                        Created = DateTime.UtcNow
                                    });
                                }
                            }
                            else
                            {
                                fiasInstallationManagerService.AddInstallation(new FIASVersionInstallation()
                                {
                                    Id = Guid.NewGuid(),
                                    FIASVersionId = newFIASVersion.Id,
                                    StatusId = FIASVersionInstallationStatus.New,
                                    InstallationTypeId = FIASVersionInstallationType.Update,
                                    Created = DateTime.UtcNow
                                });
                            }

                            await fiasInstallationManagerService.SaveAsync();
                        }

                        await fiasMaintenanceService.CommitTransactionAsync();
                    }
                    
                    _logger.LogInformation($"Окончание проверки версии ФИАС: {fiasActualDistributionInfo.VersionId}.");
                }
            }

            _logger.LogInformation("Завершена акуализация информации о версиях ФИАС.");
        }
    }
}
