using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
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
            IFIASDistributionBrowser fiasDistributionBrowser = scope.ServiceProvider.GetRequiredService<IFIASDistributionBrowser>();
            IFIASMaintenanceRepository fiasMaintenanceService = scope.ServiceProvider.GetRequiredService<IFIASMaintenanceRepository>();
            IFIASInstallationManagerRepository fiasInstallationManagerService = scope.ServiceProvider.GetRequiredService<IFIASInstallationManagerRepository>();
            IFIASBaseCatalogsRepository fiasBaseCatalogsRepository = scope.ServiceProvider.GetRequiredService<IFIASBaseCatalogsRepository>();

            // TODO Если статус активной установки не связан с активным процессом установки или обновления, то статус нужно сбросить
            var activeInstallations = await fiasInstallationManagerService.GetInstallations(
                statusId: FIASVersionInstallationStatus.Installing);
            if (activeInstallations.Count > 0)
            {
                _logger.LogWarning(
                    ".Обнаружена активная установка (процесс установки или обновления) классификатора ФИАС. Задание прервано.");
                _logger.LogWarning(
                    $"Идентификатор активной установки: {activeInstallations.First().Id}.");
                return;
            }
            
            var preparedInstallations = await fiasInstallationManagerService.GetInstallations(
                statusId: FIASVersionInstallationStatus.New);
            if (preparedInstallations.Any())
            {
                var installationToExecute = preparedInstallations.First();
                
                installationToExecute.StatusId = FIASVersionInstallationStatus.Installing;
                fiasInstallationManagerService.UpdateInstallation(installationToExecute);
                await fiasInstallationManagerService.SaveAsync();
                
                // 1. В зависимости от типа установки скачиваем файл дистрибутива ФИАС
                // 1.1. Если файл дистрибутива уже был успешно скачен ранее, то пропускаем шаг.
                // 1.1.1. Получаем информацию о версии ФИАС
                var fiasVersionInfo = await fiasMaintenanceService.GetVersion(installationToExecute.FIASVersionId);
                // 1.1.2. Получаем последнюю информацию по версии ФИАС
                var lastFIASVersionInfo = await fiasMaintenanceService.GetLastVersion(fiasVersionInfo.VersionId);
                // 1.1.3. Получаем все дистрибутивы ФИАС, доступные через API и среди них находим нужных объект
                var allDistributions = await fiasDistributionBrowser.GetAllDistributionInfo();
                var distribution = allDistributions.First(e => e.VersionId == lastFIASVersionInfo.VersionId);
                // 1.1.4 Скачиваем нужный файл дистрибутива
                DistributionFileType distributionFileType;
                if (installationToExecute.InstallationTypeId == FIASVersionInstallationType.Full)
                    distributionFileType = DistributionFileType.GARFIASXmlComplete;
                else
                    distributionFileType = DistributionFileType.GARFIASXmlDelta;
                await distribution.DownloadDistributionByFileTypeAsync(distributionFileType);

                // 2. Распаковываем файлы базовых справочников (если уже были ранее распакованы, то повторяем операцию)
                distribution.ExtractDistributionFile(distributionFileType, true);
                string distributionDirectory = distribution.GetExtractedDirectory(distributionFileType);

                // 3. Читаем данные файлов базовых справочников
                // 3.1. Сохранем элементы базовых справочников в базу данных (новые, измененные, удаленные).
                IFIASDistributionReader distributionReader = new FIASDistributionReader(distributionDirectory);

                // TODO Перенести загрузку данных ФИАС в отдельный проект
                
                #region AddressObjectType
                
                var fiasAddressObjectTypes = distributionReader.GetAddressObjectTypes();
                foreach (var fiasAddressObjectType in fiasAddressObjectTypes)
                {
                    var addressObjectType = await fiasBaseCatalogsRepository.GetAddressObjectType(fiasAddressObjectType.Id);
                    if (addressObjectType == null)
                    {
                        addressObjectType = new AddressObjectType();
                        addressObjectType.Id = fiasAddressObjectType.Id;
                        fiasBaseCatalogsRepository.AddAddressObjectType(addressObjectType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateAddressObjectType(addressObjectType);
                    }

                    addressObjectType.Level = fiasAddressObjectType.Level;
                    addressObjectType.Name = fiasAddressObjectType.Name;
                    addressObjectType.ShortName = fiasAddressObjectType.ShortName;
                    addressObjectType.Description = fiasAddressObjectType.Description;
                    addressObjectType.StartDate = fiasAddressObjectType.StartDate.ToDateTime(TimeOnly.MinValue);
                    addressObjectType.EndDate = fiasAddressObjectType.EndDate.ToDateTime(TimeOnly.MinValue);
                    addressObjectType.UpdateDate = fiasAddressObjectType.UpdateDate.ToDateTime(TimeOnly.MinValue);
                }

                var fiasAddressObjectTypesFromDatabase = await fiasBaseCatalogsRepository.GetAddressObjectTypes();
                foreach (var fiasAddressObjectTypeFromDatabase in fiasAddressObjectTypesFromDatabase)
                {
                    if (fiasAddressObjectTypes.All(e => e.Id != fiasAddressObjectTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveAddressObjectType(fiasAddressObjectTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<AddressObjectType>();

                #endregion
                
                #region ApartmentType
                
                var fiasApartmentTypes = distributionReader.GetApartmentTypes();
                foreach (var fiasApartmentType in fiasApartmentTypes)
                {
                    var apartmentType = await fiasBaseCatalogsRepository.GetApartmentType(fiasApartmentType.Id);
                    if (apartmentType == null)
                    {
                        apartmentType = new ApartmentType();
                        apartmentType.Id = fiasApartmentType.Id;
                        fiasBaseCatalogsRepository.AddApartmentType(apartmentType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateApartmentType(apartmentType);
                    }
                    
                    apartmentType.Name = fiasApartmentType.Name;
                    apartmentType.ShortName = fiasApartmentType.ShortName;
                    apartmentType.Description = fiasApartmentType.Description;
                    apartmentType.StartDate = fiasApartmentType.StartDate.ToDateTime(TimeOnly.MinValue);
                    apartmentType.EndDate = fiasApartmentType.EndDate.ToDateTime(TimeOnly.MinValue);
                    apartmentType.UpdateDate = fiasApartmentType.UpdateDate.ToDateTime(TimeOnly.MinValue);
                    apartmentType.IsActive = fiasApartmentType.IsActive;
                }

                var fiasApartmentTypesFromDatabase = await fiasBaseCatalogsRepository.GetApartmentTypes();
                foreach (var fiasApartmentTypeFromDatabase in fiasApartmentTypesFromDatabase)
                {
                    if (fiasApartmentTypes.All(e => e.Id != fiasApartmentTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveApartmentType(fiasApartmentTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<ApartmentType>();

                #endregion
                
                #region HouseType
                
                var fiasHouseTypes = distributionReader.GetHouseTypes();
                foreach (var fiasHouseType in fiasHouseTypes)
                {
                    var houseType = await fiasBaseCatalogsRepository.GetHouseType(fiasHouseType.Id);
                    if (houseType == null)
                    {
                        houseType = new HouseType();
                        houseType.Id = fiasHouseType.Id;
                        fiasBaseCatalogsRepository.AddHouseType(houseType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateHouseType(houseType);
                    }
                    
                    houseType.Name = fiasHouseType.Name;
                    houseType.ShortName = fiasHouseType.ShortName;
                    houseType.Description = fiasHouseType.Description;
                    houseType.StartDate = fiasHouseType.StartDate.ToDateTime(TimeOnly.MinValue);
                    houseType.EndDate = fiasHouseType.EndDate.ToDateTime(TimeOnly.MinValue);
                    houseType.UpdateDate = fiasHouseType.UpdateDate.ToDateTime(TimeOnly.MinValue);
                    houseType.IsActive = fiasHouseType.IsActive;
                }

                var fiasHouseTypesFromDatabase = await fiasBaseCatalogsRepository.GetHouseTypes();
                foreach (var fiasHouseTypeFromDatabase in fiasHouseTypesFromDatabase)
                {
                    if (fiasHouseTypes.All(e => e.Id != fiasHouseTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveHouseType(fiasHouseTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<HouseType>();

                #endregion
                
                #region NormativeDocKind
                
                var fiasNormativeDocKinds = distributionReader.GetNormativeDocKinds();
                foreach (var fiasNormativeDocKind in fiasNormativeDocKinds)
                {
                    if(fiasNormativeDocKind.Id == 0)
                        continue;
                    
                    var normativeDocKind = await fiasBaseCatalogsRepository.GetNormativeDocKind(fiasNormativeDocKind.Id);
                    if (normativeDocKind == null)
                    {
                        normativeDocKind = new NormativeDocKind();
                        normativeDocKind.Id = fiasNormativeDocKind.Id;
                        fiasBaseCatalogsRepository.AddNormativeDocKind(normativeDocKind);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateNormativeDocKind(normativeDocKind);
                    }
                    
                    normativeDocKind.Name = fiasNormativeDocKind.Name;
                }

                var fiasNormativeDocKindsFromDatabase = await fiasBaseCatalogsRepository.GetNormativeDocKinds();
                foreach (var fiasNormativeDocKindFromDatabase in fiasNormativeDocKindsFromDatabase)
                {
                    if (fiasNormativeDocKinds.All(e => e.Id != fiasNormativeDocKindFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveNormativeDocKind(fiasNormativeDocKindFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<NormativeDocKind>();

                #endregion
                
                #region NormativeDocType
                
                var fiasNormativeDocTypes = distributionReader.GetNormativeDocTypes();
                foreach (var fiasNormativeDocType in fiasNormativeDocTypes)
                {
                    if(fiasNormativeDocType.Id == 0)
                        continue;
                    
                    var normativeDocType = await fiasBaseCatalogsRepository.GetNormativeDocType(fiasNormativeDocType.Id);
                    if (normativeDocType == null)
                    {
                        normativeDocType = new NormativeDocType();
                        normativeDocType.Id = fiasNormativeDocType.Id;
                        fiasBaseCatalogsRepository.AddNormativeDocType(normativeDocType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateNormativeDocType(normativeDocType);
                    }
                    
                    normativeDocType.Name = fiasNormativeDocType.Name;
                    normativeDocType.StartDate = fiasNormativeDocType.StartDate.ToDateTime(TimeOnly.MinValue);
                    normativeDocType.EndDate = fiasNormativeDocType.EndDate.ToDateTime(TimeOnly.MinValue);
                }

                var fiasNormativeDocTypesFromDatabase = await fiasBaseCatalogsRepository.GetNormativeDocTypes();
                foreach (var fiasNormativeDocTypeFromDatabase in fiasNormativeDocTypesFromDatabase)
                {
                    if (fiasNormativeDocTypes.All(e => e.Id != fiasNormativeDocTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveNormativeDocType(fiasNormativeDocTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<NormativeDocType>();

                #endregion
                
                #region ObjectLevel
                
                var fiasObjectLevels = distributionReader.GetObjectLevels();
                foreach (var fiasObjectLevel in fiasObjectLevels)
                {
                    var objectLevel = await fiasBaseCatalogsRepository.GetObjectLevel(fiasObjectLevel.Level);
                    if (objectLevel == null)
                    {
                        objectLevel = new ObjectLevel();
                        objectLevel.Level = fiasObjectLevel.Level;
                        fiasBaseCatalogsRepository.AddObjectLevel(objectLevel);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateObjectLevel(objectLevel);
                    }

                    objectLevel.Name = fiasObjectLevel.Name;
                    objectLevel.StartDate = fiasObjectLevel.StartDate.ToDateTime(TimeOnly.MinValue);
                    objectLevel.EndDate = fiasObjectLevel.EndDate.ToDateTime(TimeOnly.MinValue);
                    objectLevel.UpdateDate = fiasObjectLevel.UpdateDate.ToDateTime(TimeOnly.MinValue);
                    objectLevel.IsActive = fiasObjectLevel.IsActive;
                }

                var fiasObjectLevelsFromDatabase = await fiasBaseCatalogsRepository.GetObjectLevels();
                foreach (var fiasObjectLevelFromDatabase in fiasObjectLevelsFromDatabase)
                {
                    if (fiasObjectLevels.All(e => e.Level != fiasObjectLevelFromDatabase.Level))
                    {
                        fiasBaseCatalogsRepository.RemoveObjectLevel(fiasObjectLevelFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<ObjectLevel>();

                #endregion

                #region OperationType
                
                var fiasOperationTypes = distributionReader.GetOperationTypes();
                foreach (var fiasOperationType in fiasOperationTypes)
                {
                    if(fiasOperationType.Id == 0)
                        continue;
                    
                    var operationType = await fiasBaseCatalogsRepository.GetOperationType(fiasOperationType.Id);
                    if (operationType == null)
                    {
                        operationType = new OperationType();
                        operationType.Id = fiasOperationType.Id;
                        fiasBaseCatalogsRepository.AddOperationType(operationType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateOperationType(operationType);
                    }

                    operationType.Name = fiasOperationType.Name;
                    operationType.StartDate = fiasOperationType.StartDate.ToDateTime(TimeOnly.MinValue);
                    operationType.EndDate = fiasOperationType.EndDate.ToDateTime(TimeOnly.MinValue);
                    operationType.UpdateDate = fiasOperationType.UpdateDate.ToDateTime(TimeOnly.MinValue);
                    operationType.IsActive = fiasOperationType.IsActive;
                }

                var fiasOperationTypesFromDatabase = await fiasBaseCatalogsRepository.GetOperationTypes();
                foreach (var fiasOperationTypeFromDatabase in fiasOperationTypesFromDatabase)
                {
                    if (fiasOperationTypes.All(e => e.Id != fiasOperationTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveOperationType(fiasOperationTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<OperationType>();

                #endregion
                
                #region ParameterType
                
                var fiasParameterTypes = distributionReader.GetParameterTypes();
                foreach (var fiasParameterType in fiasParameterTypes)
                {
                    if(fiasParameterType.Id == 0)
                        continue;
                    
                    var parameterType = await fiasBaseCatalogsRepository.GetParameterType(fiasParameterType.Id);
                    if (parameterType == null)
                    {
                        parameterType = new ParameterType();
                        parameterType.Id = fiasParameterType.Id;
                        fiasBaseCatalogsRepository.AddParameterType(parameterType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateParameterType(parameterType);
                    }

                    parameterType.Name = fiasParameterType.Name;
                    parameterType.Description = fiasParameterType.Description;
                    parameterType.Code = fiasParameterType.Code;
                    parameterType.StartDate = fiasParameterType.StartDate.ToDateTime(TimeOnly.MinValue);
                    parameterType.EndDate = fiasParameterType.EndDate.ToDateTime(TimeOnly.MinValue);
                    parameterType.UpdateDate = fiasParameterType.UpdateDate.ToDateTime(TimeOnly.MinValue);
                    parameterType.IsActive = fiasParameterType.IsActive;
                }

                var fiasParameterTypesFromDatabase = await fiasBaseCatalogsRepository.GetParameterTypes();
                foreach (var fiasParameterTypeFromDatabase in fiasParameterTypesFromDatabase)
                {
                    if (fiasParameterTypes.All(e => e.Id != fiasParameterTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveParameterType(fiasParameterTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<ParameterType>();

                #endregion
                
                #region RoomType
                
                var fiasRoomTypes = distributionReader.GetRoomTypes();
                foreach (var fiasRoomType in fiasRoomTypes)
                {
                    if(fiasRoomType.Id == 0)
                        continue;
                    
                    var roomType = await fiasBaseCatalogsRepository.GetRoomType(fiasRoomType.Id);
                    if (roomType == null)
                    {
                        roomType = new RoomType();
                        roomType.Id = fiasRoomType.Id;
                        fiasBaseCatalogsRepository.AddRoomType(roomType);
                    }
                    else
                    {
                        fiasBaseCatalogsRepository.UpdateRoomType(roomType);
                    }

                    roomType.Name = fiasRoomType.Name;
                    roomType.Description = fiasRoomType.Description;
                    roomType.StartDate = fiasRoomType.StartDate.ToDateTime(TimeOnly.MinValue);
                    roomType.EndDate = fiasRoomType.EndDate.ToDateTime(TimeOnly.MinValue);
                    roomType.UpdateDate = fiasRoomType.UpdateDate.ToDateTime(TimeOnly.MinValue);
                    roomType.IsActive = fiasRoomType.IsActive;
                }

                var fiasRoomTypesFromDatabase = await fiasBaseCatalogsRepository.GetRoomTypes();
                foreach (var fiasRoomTypeFromDatabase in fiasRoomTypesFromDatabase)
                {
                    if (fiasRoomTypes.All(e => e.Id != fiasRoomTypeFromDatabase.Id))
                    {
                        fiasBaseCatalogsRepository.RemoveRoomType(fiasRoomTypeFromDatabase);
                    }
                }
                
                await fiasBaseCatalogsRepository.SaveWithIdentityInsertAsync<RoomType>();

                #endregion
                
                installationToExecute.StatusId = FIASVersionInstallationStatus.Installed;
                fiasInstallationManagerService.UpdateInstallation(installationToExecute);
                await fiasInstallationManagerService.SaveAsync();
            }
        }

        _logger.LogInformation("Завершена обработка шагов по установке / обновлению классификатора ФИАС.");
    }
}