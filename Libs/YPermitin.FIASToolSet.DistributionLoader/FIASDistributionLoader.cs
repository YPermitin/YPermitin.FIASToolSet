using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.DistributionLoader.Exceptions;
using YPermitin.FIASToolSet.DistributionLoader.Extensions;
using YPermitin.FIASToolSet.DistributionLoader.Models;
using YPermitin.FIASToolSet.DistributionReader;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;
using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.DistributionLoader;

public class FIASDistributionLoader : IFIASDistributionLoader
{
    private readonly IFIASDistributionBrowser _fiasDistributionBrowser;
    private readonly IFIASInstallationManagerRepository _fiasInstallationManagerService;
    private readonly IFIASBaseCatalogsRepository _fiasBaseCatalogsRepository;
    private readonly IFIASMaintenanceRepository _fiasMaintenanceService;
    private readonly IFIASClassifierDataRepository _classifierDataRepository;
    
    private FIASVersionInstallation _installation;
    private string _distributionDirectory;
    private IFIASDistributionReader _distributionReader;

    public FIASVersionInstallation VersionInstallation => _installation;

    public FIASDistributionInfo Distribution { get; private set; }
    public FIASVersion CurrentVersion { get; private set; }

    public FIASDistributionLoader(
        IFIASDistributionBrowser fiasDistributionBrowser,
        IFIASInstallationManagerRepository fiasInstallationManagerService, 
        IFIASBaseCatalogsRepository fiasBaseCatalogsRepository, 
        IFIASMaintenanceRepository fiasMaintenanceService,
        IFIASClassifierDataRepository classifierDataRepository)
    {
        _fiasDistributionBrowser = fiasDistributionBrowser;
        _fiasInstallationManagerService = fiasInstallationManagerService;
        _fiasBaseCatalogsRepository = fiasBaseCatalogsRepository;
        _fiasMaintenanceService = fiasMaintenanceService;
        _classifierDataRepository = classifierDataRepository;
    }

    public async Task<bool> ActiveInstallationExists()
    {
        var activeInstallations = await _fiasInstallationManagerService.GetInstallations(
            statusId: FIASVersionInstallationStatus.Installing);

        return activeInstallations.Count > 0;
    }
    
    public async Task<List<FIASVersionInstallation>> FixStuckInstallationExists()
    {
        List<FIASVersionInstallation> stuckInstallations = new List<FIASVersionInstallation>();

        var activeInstallations = await _fiasInstallationManagerService.GetInstallations(
            statusId: FIASVersionInstallationStatus.Installing);

        foreach (var activeInstallation in activeInstallations)
        {
            if (activeInstallation.StartDate != null)
            {
                var startTimeLeft = DateTime.UtcNow - (DateTime)activeInstallation.StartDate;
                if (startTimeLeft.TotalHours >= 4)
                {
                    activeInstallation.StartDate = null;
                    activeInstallation.FinishDate = null;
                    activeInstallation.StatusId = FIASVersionInstallationStatus.New;
                    _fiasInstallationManagerService.UpdateInstallation(activeInstallation);

                    stuckInstallations.Add(activeInstallation);
                }
            }
        }

        await _fiasInstallationManagerService.SaveAsync();

        return stuckInstallations;
    }

    public async Task<bool> InitVersionInstallationToLoad()
    {
        var preparedInstallations = await _fiasInstallationManagerService.GetInstallations(
            statusId: FIASVersionInstallationStatus.New,
            includeDetails: true);

        _distributionDirectory = null;
        _distributionReader = null;
        _installation = null;
        CurrentVersion = null;
        Distribution = null;
        
        if (preparedInstallations.Any())
        {
            _installation = preparedInstallations.First();
            CurrentVersion = _installation.FIASVersion;
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task DownloadAndExtractDistribution(
        Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null)
    {
        // 1. В зависимости от типа установки скачиваем файл дистрибутива ФИАС
        // 1.1. Если файл дистрибутива уже был успешно скачен ранее, то пропускаем шаг.
        // 1.1.1. Получаем информацию о версии ФИАС
        var fiasVersionInfo = await _fiasMaintenanceService.GetVersion(_installation.FIASVersionId);
        // 1.1.2. Получаем последнюю информацию по версии ФИАС
        var lastFIASVersionInfo = await _fiasMaintenanceService.GetLastVersion(fiasVersionInfo.VersionId);
        CurrentVersion = lastFIASVersionInfo;
        
        // 1.1.3. Получаем все дистрибутивы ФИАС, доступные через API и среди них находим нужных объект
        var allDistributions = await _fiasDistributionBrowser.GetAllDistributionInfo();
        var distribution = allDistributions.First(e => e.VersionId == lastFIASVersionInfo.VersionId);
        Distribution = distribution;
        
        // 1.1.4 Скачиваем нужный файл дистрибутива
        DistributionFileType distributionFileType;
        if (_installation.InstallationTypeId == FIASVersionInstallationType.Full)
            distributionFileType = DistributionFileType.GARFIASXmlComplete;
        else
            distributionFileType = DistributionFileType.GARFIASXmlDelta;
        await Distribution.DownloadDistributionByFileTypeAsync(distributionFileType, onDownloadFileProgressChangedEvent);

        // 2. Распаковываем файлы базовых справочников (если уже были ранее распакованы, то повторяем операцию)
        distribution.ExtractDistributionFile(distributionFileType);
        _distributionDirectory = Distribution.GetExtractedDirectory(distributionFileType);
    }

    /// <summary>
    /// Удалениие архива данных для версии ФИАС
    /// </summary>
    public void RemoveVersionDataArchive()
    {
        DistributionFileType distributionFileType;
        if (_installation.InstallationTypeId == FIASVersionInstallationType.Full)
            distributionFileType = DistributionFileType.GARFIASXmlComplete;
        else
            distributionFileType = DistributionFileType.GARFIASXmlDelta;
        
        Distribution.RemoveVersionDataArchive(distributionFileType);
    }

    /// <summary>
    /// Удаление каталога данных для версии ФИАС
    /// </summary>
    public void RemoveVersionDataDirectory()
    {
        if (Directory.Exists(_distributionDirectory))
        {
            Directory.Delete(_distributionDirectory, true);
        }
    }

    /// <summary>
    /// Получает список кодов регионов, доступных для распаковки данных и загрузки
    /// </summary>
    /// <returns>Коллекция доступных регионов</returns>
    public List<Region> GetAvailableRegions()
    {
        DistributionFileType distributionFileType;
        if (_installation.InstallationTypeId == FIASVersionInstallationType.Full)
            distributionFileType = DistributionFileType.GARFIASXmlComplete;
        else
            distributionFileType = DistributionFileType.GARFIASXmlDelta;
        
        var availableRegions = Distribution.GetAvailableRegions(distributionFileType)
            .Select(e => new Region(int.Parse(e)))
            .ToList();

        return availableRegions;
    }
    
    /// <summary>
    /// Распаковка данных для указанного региона
    /// </summary>
    /// <param name="region">Регион для распаковки данных</param>
    /// <returns>Путь к каталогу с данными по региону</returns>
    /// <exception cref="RegionNotFoundException">Регион с указанным кодом не найден</exception>
    public string ExtractDataForRegion(Region region)
    {
        DistributionFileType distributionFileType;
        if (_installation.InstallationTypeId == FIASVersionInstallationType.Full)
            distributionFileType = DistributionFileType.GARFIASXmlComplete;
        else
            distributionFileType = DistributionFileType.GARFIASXmlDelta;
        
        var availableRegions = Distribution.GetAvailableRegions(distributionFileType);
        var regionItem = availableRegions.FirstOrDefault(e => e == region.Code.ToString());
        if (regionItem == null)
        {
            throw new RegionNotFoundException(
                $"Не найден регион с кодом \"{region}\" среди достпных регионов в дистрибутиве ФИАС.",
                region.ToString());
        }
        
        Distribution.ExtractDistributionRegionFiles(distributionFileType, region.Code.ToString());

        return GetDataDirectoryForRegion(region);
    }

    /// <summary>
    /// Полусение пути к каталогу с данными региона
    /// </summary>
    /// <param name="region">Регион для распаковки данных</param>
    /// <returns>Путь к каталогу с данными по региону</returns>
    public string GetDataDirectoryForRegion(Region region)
    {
        return Path.Combine(_distributionDirectory, region.Code.ToString());
    }

    /// <summary>
    /// Удаление каталога с данными классификатора по региону
    /// </summary>
    /// <param name="region">Регион для удаления каталога данных</param>
    public void RemoveDistributionRegionDirectory(Region region)
    {
        DistributionFileType distributionFileType;
        if (_installation.InstallationTypeId == FIASVersionInstallationType.Full)
            distributionFileType = DistributionFileType.GARFIASXmlComplete;
        else
            distributionFileType = DistributionFileType.GARFIASXmlDelta;
        
        Distribution.RemoveDistributionRegionDirectory(distributionFileType, region.Code.ToString());
    }
    
    public async Task SetInstallationToStatusNew()
    {
        _installation.StatusId = FIASVersionInstallationStatus.New;
        _installation.StartDate = null;
        _installation.FinishDate = null;
        _fiasInstallationManagerService.UpdateInstallation(_installation);
        await _fiasInstallationManagerService.SaveAsync();
    }
    
    public async Task SetInstallationToStatusInstalling()
    {
        _installation.StatusId = FIASVersionInstallationStatus.Installing;
        _installation.StartDate = DateTime.UtcNow;
        _fiasInstallationManagerService.UpdateInstallation(_installation);
        await _fiasInstallationManagerService.SaveAsync();
    }
    
    public async Task SetInstallationToStatusInstalled()
    {
        _installation.StatusId = FIASVersionInstallationStatus.Installed;
        _installation.FinishDate = DateTime.UtcNow;
        _fiasInstallationManagerService.UpdateInstallation(_installation);
        await _fiasInstallationManagerService.SaveAsync();
    }

    #region BaseCatalogs
    
    public async Task LoadAddressObjectTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        var fiasAddressObjectTypes = fiasDistributionReader.GetAddressObjectTypes();
        foreach (var fiasAddressObjectType in fiasAddressObjectTypes)
        {
            var addressObjectType = await _fiasBaseCatalogsRepository.GetAddressObjectType(fiasAddressObjectType.Id);
            if (addressObjectType == null)
            {
                addressObjectType = new AddressObjectType();
                addressObjectType.Id = fiasAddressObjectType.Id;
                _fiasBaseCatalogsRepository.AddAddressObjectType(addressObjectType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateAddressObjectType(addressObjectType);
            }

            addressObjectType.Level = fiasAddressObjectType.Level;
            addressObjectType.Name = fiasAddressObjectType.Name;
            addressObjectType.ShortName = fiasAddressObjectType.ShortName;
            addressObjectType.Description = fiasAddressObjectType.Description;
            addressObjectType.StartDate = fiasAddressObjectType.StartDate.ToDateTime(TimeOnly.MinValue);
            addressObjectType.EndDate = fiasAddressObjectType.EndDate.ToDateTime(TimeOnly.MinValue);
            addressObjectType.UpdateDate = fiasAddressObjectType.UpdateDate.ToDateTime(TimeOnly.MinValue);
        }

        var fiasAddressObjectTypesFromDatabase = await _fiasBaseCatalogsRepository.GetAddressObjectTypes();
        foreach (var fiasAddressObjectTypeFromDatabase in fiasAddressObjectTypesFromDatabase)
        {
            if (fiasAddressObjectTypes.All(e => e.Id != fiasAddressObjectTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveAddressObjectType(fiasAddressObjectTypeFromDatabase);
            }
        }

        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadApartmentTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        var fiasApartmentTypes = fiasDistributionReader.GetApartmentTypes();
        foreach (var fiasApartmentType in fiasApartmentTypes)
        {
            var apartmentType = await _fiasBaseCatalogsRepository.GetApartmentType(fiasApartmentType.Id);
            if (apartmentType == null)
            {
                apartmentType = new ApartmentType();
                apartmentType.Id = fiasApartmentType.Id;
                _fiasBaseCatalogsRepository.AddApartmentType(apartmentType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateApartmentType(apartmentType);
            }
                    
            apartmentType.Name = fiasApartmentType.Name;
            apartmentType.ShortName = fiasApartmentType.ShortName;
            apartmentType.Description = fiasApartmentType.Description;
            apartmentType.StartDate = fiasApartmentType.StartDate.ToDateTime(TimeOnly.MinValue);
            apartmentType.EndDate = fiasApartmentType.EndDate.ToDateTime(TimeOnly.MinValue);
            apartmentType.UpdateDate = fiasApartmentType.UpdateDate.ToDateTime(TimeOnly.MinValue);
            apartmentType.IsActive = fiasApartmentType.IsActive;
        }

        var fiasApartmentTypesFromDatabase = await _fiasBaseCatalogsRepository.GetApartmentTypes();
        foreach (var fiasApartmentTypeFromDatabase in fiasApartmentTypesFromDatabase)
        {
            if (fiasApartmentTypes.All(e => e.Id != fiasApartmentTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveApartmentType(fiasApartmentTypeFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadHouseTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        var fiasHouseTypes = fiasDistributionReader.GetHouseTypes();
        foreach (var fiasHouseType in fiasHouseTypes)
        {
            var houseType = await _fiasBaseCatalogsRepository.GetHouseType(fiasHouseType.Id);
            if (houseType == null)
            {
                houseType = new HouseType();
                houseType.Id = fiasHouseType.Id;
                _fiasBaseCatalogsRepository.AddHouseType(houseType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateHouseType(houseType);
            }
                    
            houseType.Name = fiasHouseType.Name;
            houseType.ShortName = fiasHouseType.ShortName;
            houseType.Description = fiasHouseType.Description;
            houseType.StartDate = fiasHouseType.StartDate.ToDateTime(TimeOnly.MinValue);
            houseType.EndDate = fiasHouseType.EndDate.ToDateTime(TimeOnly.MinValue);
            houseType.UpdateDate = fiasHouseType.UpdateDate.ToDateTime(TimeOnly.MinValue);
            houseType.IsActive = fiasHouseType.IsActive;
        }

        var fiasHouseTypesFromDatabase = await _fiasBaseCatalogsRepository.GetHouseTypes();
        foreach (var fiasHouseTypeFromDatabase in fiasHouseTypesFromDatabase)
        {
            if (fiasHouseTypes.All(e => e.Id != fiasHouseTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveHouseType(fiasHouseTypeFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadNormativeDocKinds()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        var fiasNormativeDocKinds = fiasDistributionReader.GetNormativeDocKinds();
        foreach (var fiasNormativeDocKind in fiasNormativeDocKinds)
        {
            var normativeDocKind = await _fiasBaseCatalogsRepository.GetNormativeDocKind(fiasNormativeDocKind.Id);
            if (normativeDocKind == null)
            {
                normativeDocKind = new NormativeDocKind();
                normativeDocKind.Id = fiasNormativeDocKind.Id;
                _fiasBaseCatalogsRepository.AddNormativeDocKind(normativeDocKind);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateNormativeDocKind(normativeDocKind);
            }
                    
            normativeDocKind.Name = fiasNormativeDocKind.Name;
        }

        var fiasNormativeDocKindsFromDatabase = await _fiasBaseCatalogsRepository.GetNormativeDocKinds();
        foreach (var fiasNormativeDocKindFromDatabase in fiasNormativeDocKindsFromDatabase)
        {
            if (fiasNormativeDocKinds.All(e => e.Id != fiasNormativeDocKindFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveNormativeDocKind(fiasNormativeDocKindFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadNormativeDocTypes()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        var fiasNormativeDocTypes = fiasDistributionReader.GetNormativeDocTypes();
        foreach (var fiasNormativeDocType in fiasNormativeDocTypes)
        {
            var normativeDocType = await _fiasBaseCatalogsRepository.GetNormativeDocType(fiasNormativeDocType.Id);
            if (normativeDocType == null)
            {
                normativeDocType = new NormativeDocType();
                normativeDocType.Id = fiasNormativeDocType.Id;
                _fiasBaseCatalogsRepository.AddNormativeDocType(normativeDocType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateNormativeDocType(normativeDocType);
            }
                    
            normativeDocType.Name = fiasNormativeDocType.Name;
            normativeDocType.StartDate = fiasNormativeDocType.StartDate.ToDateTime(TimeOnly.MinValue);
            normativeDocType.EndDate = fiasNormativeDocType.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        var fiasNormativeDocTypesFromDatabase = await _fiasBaseCatalogsRepository.GetNormativeDocTypes();
        foreach (var fiasNormativeDocTypeFromDatabase in fiasNormativeDocTypesFromDatabase)
        {
            if (fiasNormativeDocTypes.All(e => e.Id != fiasNormativeDocTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveNormativeDocType(fiasNormativeDocTypeFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadObjectLevels()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        var fiasObjectLevels = fiasDistributionReader.GetObjectLevels();
        foreach (var fiasObjectLevel in fiasObjectLevels)
        {
            var objectLevel = await _fiasBaseCatalogsRepository.GetObjectLevel(fiasObjectLevel.Level);
            if (objectLevel == null)
            {
                objectLevel = new ObjectLevel();
                objectLevel.Level = fiasObjectLevel.Level;
                _fiasBaseCatalogsRepository.AddObjectLevel(objectLevel);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateObjectLevel(objectLevel);
            }

            objectLevel.Name = fiasObjectLevel.Name;
            objectLevel.StartDate = fiasObjectLevel.StartDate.ToDateTime(TimeOnly.MinValue);
            objectLevel.EndDate = fiasObjectLevel.EndDate.ToDateTime(TimeOnly.MinValue);
            objectLevel.UpdateDate = fiasObjectLevel.UpdateDate.ToDateTime(TimeOnly.MinValue);
            objectLevel.IsActive = fiasObjectLevel.IsActive;
        }

        var fiasObjectLevelsFromDatabase = await _fiasBaseCatalogsRepository.GetObjectLevels();
        foreach (var fiasObjectLevelFromDatabase in fiasObjectLevelsFromDatabase)
        {
            if (fiasObjectLevels.All(e => e.Level != fiasObjectLevelFromDatabase.Level))
            {
                _fiasBaseCatalogsRepository.RemoveObjectLevel(fiasObjectLevelFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadOperationTypes()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        var fiasOperationTypes = fiasDistributionReader.GetOperationTypes();
        foreach (var fiasOperationType in fiasOperationTypes)
        {
            var operationType = await _fiasBaseCatalogsRepository.GetOperationType(fiasOperationType.Id);
            if (operationType == null)
            {
                operationType = new OperationType();
                operationType.Id = fiasOperationType.Id;
                _fiasBaseCatalogsRepository.AddOperationType(operationType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateOperationType(operationType);
            }

            operationType.Name = fiasOperationType.Name;
            operationType.StartDate = fiasOperationType.StartDate.ToDateTime(TimeOnly.MinValue);
            operationType.EndDate = fiasOperationType.EndDate.ToDateTime(TimeOnly.MinValue);
            operationType.UpdateDate = fiasOperationType.UpdateDate.ToDateTime(TimeOnly.MinValue);
            operationType.IsActive = fiasOperationType.IsActive;
        }

        var fiasOperationTypesFromDatabase = await _fiasBaseCatalogsRepository.GetOperationTypes();
        foreach (var fiasOperationTypeFromDatabase in fiasOperationTypesFromDatabase)
        {
            if (fiasOperationTypes.All(e => e.Id != fiasOperationTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveOperationType(fiasOperationTypeFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }

    public async Task LoadParameterTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        var fiasParameterTypes = fiasDistributionReader.GetParameterTypes();
        foreach (var fiasParameterType in fiasParameterTypes)
        {
            var parameterType = await _fiasBaseCatalogsRepository.GetParameterType(fiasParameterType.Id);
            if (parameterType == null)
            {
                parameterType = new ParameterType();
                parameterType.Id = fiasParameterType.Id;
                _fiasBaseCatalogsRepository.AddParameterType(parameterType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateParameterType(parameterType);
            }

            parameterType.Name = fiasParameterType.Name;
            parameterType.Description = fiasParameterType.Description;
            parameterType.Code = fiasParameterType.Code;
            parameterType.StartDate = fiasParameterType.StartDate.ToDateTime(TimeOnly.MinValue);
            parameterType.EndDate = fiasParameterType.EndDate.ToDateTime(TimeOnly.MinValue);
            parameterType.UpdateDate = fiasParameterType.UpdateDate.ToDateTime(TimeOnly.MinValue);
            parameterType.IsActive = fiasParameterType.IsActive;
        }

        var fiasParameterTypesFromDatabase = await _fiasBaseCatalogsRepository.GetParameterTypes();
        foreach (var fiasParameterTypeFromDatabase in fiasParameterTypesFromDatabase)
        {
            if (fiasParameterTypes.All(e => e.Id != fiasParameterTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveParameterType(fiasParameterTypeFromDatabase);
            }
        }

        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadRoomTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        var fiasRoomTypes = fiasDistributionReader.GetRoomTypes();
        foreach (var fiasRoomType in fiasRoomTypes)
        {
            var roomType = await _fiasBaseCatalogsRepository.GetRoomType(fiasRoomType.Id);
            if (roomType == null)
            {
                roomType = new RoomType();
                roomType.Id = fiasRoomType.Id;
                _fiasBaseCatalogsRepository.AddRoomType(roomType);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateRoomType(roomType);
            }

            roomType.Name = fiasRoomType.Name;
            roomType.Description = fiasRoomType.Description;
            roomType.StartDate = fiasRoomType.StartDate.ToDateTime(TimeOnly.MinValue);
            roomType.EndDate = fiasRoomType.EndDate.ToDateTime(TimeOnly.MinValue);
            roomType.UpdateDate = fiasRoomType.UpdateDate.ToDateTime(TimeOnly.MinValue);
            roomType.IsActive = fiasRoomType.IsActive;
        }

        var fiasRoomTypesFromDatabase = await _fiasBaseCatalogsRepository.GetRoomTypes();
        foreach (var fiasRoomTypeFromDatabase in fiasRoomTypesFromDatabase)
        {
            if (fiasRoomTypes.All(e => e.Id != fiasRoomTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveRoomType(fiasRoomTypeFromDatabase);
            }
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    #endregion

    #region ClassifierData

    /// <summary>
    /// Загрузка адресных объектов по региону
    /// </summary>
    /// <param name="region">Регион для загрузки данных адресных объектов</param>
    public async Task LoadAddressObjects(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasAddressObjects = fiasDistributionReader.GetAddressObjects(fiasDistributionRegion);

        List<DistributionReader.Models.ClassifierData.AddressObject> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.AddressObject>();

        foreach (var fiasAddressObject in fiasAddressObjects)
        {
            currentPortion.Add(fiasAddressObject);

            if (currentPortion.Count == 1000)
            {
                await SaveAddressObjectsPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveAddressObjectsPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о переподчинении адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о переодчинении адресных объектов</param>
    public async Task LoadAddressObjectDivisions(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasAddressObjectDivisions = fiasDistributionReader.GetAddressObjectDivisions(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.AddressObjectDivision> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.AddressObjectDivision>();

        foreach (var fiasAddressObjectDivision in fiasAddressObjectDivisions)
        {
            currentPortion.Add(fiasAddressObjectDivision);

            if (currentPortion.Count == 1000)
            {
                await SaveAddressObjectDivisionsPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveAddressObjectDivisionsPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о параметрах адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах адресных объектов</param>
    public async Task LoadAddressObjectParameters(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasAddressObjectParameters = fiasDistributionReader.GetAddressObjectParameters(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.AddressObjectParameter> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.AddressObjectParameter>();
        
        foreach (var fiasAddressObjectParameter in fiasAddressObjectParameters)
        {
            currentPortion.Add(fiasAddressObjectParameter);

            if (currentPortion.Count == 1000)
            {
                await SaveAddressObjectParametersPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveAddressObjectParametersPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о иерархии административного деления адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о иерархии административного деления адресных объектов</param>
    public async Task LoadAddressObjectsAdmHierarchy(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasAddressObjectsAdmHierarchy = fiasDistributionReader.GetAddressObjectsAdmHierarchy(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.AddressObjectAdmHierarchy> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.AddressObjectAdmHierarchy>();
        
        foreach (var fiasAddressObjectAdmHierarchy in fiasAddressObjectsAdmHierarchy)
        {
            currentPortion.Add(fiasAddressObjectAdmHierarchy);

            if (currentPortion.Count == 1000)
            {
                await SaveAddressObjectsAdmHierarchyPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveAddressObjectsAdmHierarchyPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о квартирах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о квартирах</param>
    public async Task LoadApartments(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasApartments = fiasDistributionReader.GetApartments(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.Apartment> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.Apartment>();
        
        foreach (var fiasApartment in fiasApartments)
        {
            currentPortion.Add(fiasApartment);

            if (currentPortion.Count == 1000)
            {
                await SaveApartmentsPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveApartmentsPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о параметрах квартир
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах квартир</param>
    public async Task LoadApartmentParameters(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasApartmentParameters = fiasDistributionReader.GetApartmentParameters(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.ApartmentParameter> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.ApartmentParameter>();
        
        foreach (var fiasApartmentParameter in fiasApartmentParameters)
        {
            currentPortion.Add(fiasApartmentParameter);

            if (currentPortion.Count == 1000)
            {
                await SaveApartmentParametersPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveApartmentParametersPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о машино-местах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о машино-местах</param>
    public async Task LoadCarPlaces(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasCarPlaces = fiasDistributionReader.GetCarPlaces(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.CarPlace> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.CarPlace>();
        
        foreach (var fiasCarPlace in fiasCarPlaces)
        {
            currentPortion.Add(fiasCarPlace);

            if (currentPortion.Count == 1000)
            {
                await SaveCarPlacesPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveCarPlacesPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о параметрах машино-мест
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах машино-мест</param>
    public async Task LoadCarPlaceParameters(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasCarPlaceParameters = fiasDistributionReader.GetCarPlaceParameters(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.CarPlaceParameter> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.CarPlaceParameter>();
        
        foreach (var fiasCarPlaceParameter in fiasCarPlaceParameters)
        {
            currentPortion.Add(fiasCarPlaceParameter);

            if (currentPortion.Count == 1000)
            {
                await SaveCarPlaceParametersPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveCarPlaceParametersPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о строениях
    /// </summary>
    /// <param name="region">Регион для загрузки данных о строениях</param>
    public async Task LoadHouses(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasHouses = fiasDistributionReader.GetHouses(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.House> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.House>();
        
        foreach (var fiasHouse in fiasHouses)
        {
            currentPortion.Add(fiasHouse);

            if (currentPortion.Count == 1000)
            {
                await SaveHousesPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveHousesPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о параметрах строений
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах строений</param>
    public async Task LoadHouseParameters(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasHouseParameters = fiasDistributionReader.GetHouseParameters(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.HouseParameter> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.HouseParameter>();
        
        foreach (var fiasHouseParameter in fiasHouseParameters)
        {
            currentPortion.Add(fiasHouseParameter);

            if (currentPortion.Count == 1000)
            {
                await SaveHouseParametersPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveHouseParametersPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о комнатах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о комнатах</param>
    public async Task LoadRooms(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasRooms = fiasDistributionReader.GetRooms(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.Room> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.Room>();
        
        foreach (var fiasRoom in fiasRooms)
        {
            currentPortion.Add(fiasRoom);

            if (currentPortion.Count == 1000)
            {
                await SaveRoomsPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveRoomsPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о параметрах комнат
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах комнат</param>
    public async Task LoadRoomParameters(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasRoomParameters = fiasDistributionReader.GetRoomParameters(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.RoomParameter> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.RoomParameter>();
        
        foreach (var fiasRoomParameter in fiasRoomParameters)
        {
            currentPortion.Add(fiasRoomParameter);

            if (currentPortion.Count == 1000)
            {
                await SaveRoomParametersPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveRoomParametersPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о земельных участках
    /// </summary>
    /// <param name="region">Регион для загрузки данных о земельных участках</param>
    public async Task LoadSteads(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasSteads = fiasDistributionReader.GetSteads(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.Stead> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.Stead>();
        
        foreach (var fiasStead in fiasSteads)
        {
            currentPortion.Add(fiasStead);

            if (currentPortion.Count == 1000)
            {
                await SaveSteadsPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveSteadsPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка информации о параметрах земельных участков
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах земельных участков</param>
    public async Task LoadSteadParameters(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        var fiasSteadParameters = fiasDistributionReader.GetSteadParameters(fiasDistributionRegion);
        
        List<DistributionReader.Models.ClassifierData.SteadParameter> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.SteadParameter>();
        
        foreach (var fiasSteadParameter in fiasSteadParameters)
        {
            currentPortion.Add(fiasSteadParameter);

            if (currentPortion.Count == 1000)
            {
                await SaveSteadParametersPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveSteadParametersPortion(currentPortion);
        }
    }
    
    #endregion
    
    private IFIASDistributionReader GetDistributionReader()
    {
        if (_distributionDirectory == null || !Directory.Exists(_distributionDirectory))
        {
            throw new DistributionDirectoryNotFoundException("Не обнаружен каталог с файлами дистрибутива ФИАС", _distributionDirectory);
        }
        
        if(_distributionReader == null)
            _distributionReader = new FIASDistributionReader(_distributionDirectory);

        return _distributionReader;
    }
    
    private async Task SaveAddressObjectsPortion(List<DistributionReader.Models.ClassifierData.AddressObject> currentPortion)
    {
        DistributionReader.Models.ClassifierData.AddressObject fiasAddressObject;
        var existsAddressObjects = await _classifierDataRepository
            .GetAddressObjects(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsAddressObjects.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            AddressObject addressObject;
            if (itemToProceed.DatabaseItem == null)
            {
                addressObject = new AddressObject();
                addressObject.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddAddressObject(addressObject);
            }
            else
            {
                addressObject = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateAddressObject(addressObject);
            }

            addressObject.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            addressObject.ObjectId = itemToProceed.SourceItem.ObjectId;
            addressObject.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            addressObject.ChangeId = itemToProceed.SourceItem.ChangeId;
            addressObject.Name = itemToProceed.SourceItem.Name;
            addressObject.TypeName = itemToProceed.SourceItem.TypeName;
            addressObject.LevelId = itemToProceed.SourceItem.LevelId;
            addressObject.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
            addressObject.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId == 0
                ? null
                : itemToProceed.SourceItem.PreviousAddressObjectId;
            addressObject.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId == 0
                ? null
                : itemToProceed.SourceItem.NextAddressObjectId;
            addressObject.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            addressObject.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
            addressObject.IsActual = itemToProceed.SourceItem.IsActual;
            addressObject.IsActive = itemToProceed.SourceItem.IsActive;
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveAddressObjectDivisionsPortion(List<DistributionReader.Models.ClassifierData.AddressObjectDivision> currentPortion)
    {
        DistributionReader.Models.ClassifierData.AddressObject fiasAddressObject;
        var existsAddressObjectDivisions = await _classifierDataRepository
            .GetAddressObjectDivisions(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsAddressObjectDivisions.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            AddressObjectDivision addressObjectDivision;
            if (itemToProceed.DatabaseItem == null)
            {
                addressObjectDivision = new AddressObjectDivision();
                addressObjectDivision.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddAddressObjectDivision(addressObjectDivision);
            }
            else
            {
                addressObjectDivision = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateAddressObjectDivision(addressObjectDivision);
            }

            addressObjectDivision.ParentId = itemToProceed.SourceItem.ParentId;
            addressObjectDivision.ChildId = itemToProceed.SourceItem.ChildId;
            addressObjectDivision.ChangeId = itemToProceed.SourceItem.ChangeId;
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveAddressObjectParametersPortion(List<DistributionReader.Models.ClassifierData.AddressObjectParameter> currentPortion)
    {
        DistributionReader.Models.ClassifierData.AddressObjectParameter fiasAddressObjectParameter;
        var existsAddressObjectParameters = await _classifierDataRepository
            .GetAddressObjectParameters(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsAddressObjectParameters.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            AddressObjectParameter addressObjectParameter;
            if (itemToProceed.DatabaseItem == null)
            {
                addressObjectParameter = new AddressObjectParameter();
                addressObjectParameter.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddAddressObjectParameter(addressObjectParameter);
            }
            else
            {
                addressObjectParameter = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateAddressObjectParameter(addressObjectParameter);
            }
            
            addressObjectParameter.ObjectId = itemToProceed.SourceItem.ObjectId;
            addressObjectParameter.ChangeId = itemToProceed.SourceItem.ChangeId;
            addressObjectParameter.ChangeIdEnd = itemToProceed.SourceItem.ChangeIdEnd;
            addressObjectParameter.TypeId = itemToProceed.SourceItem.TypeId;
            addressObjectParameter.Value = itemToProceed.SourceItem.Value;
            addressObjectParameter.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            addressObjectParameter.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            addressObjectParameter.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveAddressObjectsAdmHierarchyPortion(List<DistributionReader.Models.ClassifierData.AddressObjectAdmHierarchy> currentPortion)
    {
        DistributionReader.Models.ClassifierData.AddressObjectAdmHierarchy fiasAddressObjectAdmHierarchy;
        var existsAddressObjectsAdmHierarchy = await _classifierDataRepository
            .GetAddressObjectsAdmHierarchy(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsAddressObjectsAdmHierarchy.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            AddressObjectAdmHierarchy addressObjectAdmHierarchy;
            if (itemToProceed.DatabaseItem == null)
            {
                addressObjectAdmHierarchy = new AddressObjectAdmHierarchy();
                addressObjectAdmHierarchy.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddAddressObjectAdmHierarchy(addressObjectAdmHierarchy);
            }
            else
            {
                addressObjectAdmHierarchy = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateAddressObjectAdmHierarchy(addressObjectAdmHierarchy);
            }
            
            addressObjectAdmHierarchy.ObjectId = itemToProceed.SourceItem.ObjectId;
            addressObjectAdmHierarchy.ChangeId = itemToProceed.SourceItem.ChangeId;
            addressObjectAdmHierarchy.ParentObjectId = itemToProceed.SourceItem.ParentObjectId;
            addressObjectAdmHierarchy.RegionCode = itemToProceed.SourceItem.RegionCode;
            addressObjectAdmHierarchy.AreaCode = itemToProceed.SourceItem.AreaCode;
            addressObjectAdmHierarchy.CityCode = itemToProceed.SourceItem.CityCode;
            addressObjectAdmHierarchy.PlaceCode = itemToProceed.SourceItem.PlaceCode;
            addressObjectAdmHierarchy.PlanCode = itemToProceed.SourceItem.PlanCode;
            addressObjectAdmHierarchy.StreetCode = itemToProceed.SourceItem.StreetCode;
            addressObjectAdmHierarchy.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            addressObjectAdmHierarchy.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            addressObjectAdmHierarchy.IsActive = itemToProceed.SourceItem.IsActive;
            addressObjectAdmHierarchy.Path = itemToProceed.SourceItem.Path;
            addressObjectAdmHierarchy.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            addressObjectAdmHierarchy.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            addressObjectAdmHierarchy.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveApartmentsPortion(List<DistributionReader.Models.ClassifierData.Apartment> currentPortion)
    {
        var existsApartments = await _classifierDataRepository
            .GetApartments(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsApartments.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            Apartment apartment;
            if (itemToProceed.DatabaseItem == null)
            {
                apartment = new Apartment();
                apartment.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddApartment(apartment);
            }
            else
            {
                apartment = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateApartment(apartment);
            }
            
            apartment.ObjectId = itemToProceed.SourceItem.ObjectId;
            apartment.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            apartment.ChangeId = itemToProceed.SourceItem.ChangeId;
            apartment.Number = itemToProceed.SourceItem.Number;
            apartment.ApartmentTypeId = itemToProceed.SourceItem.ApartmentTypeId;
            apartment.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
            apartment.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            apartment.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            apartment.IsActive = itemToProceed.SourceItem.IsActive;
            apartment.IsActual = itemToProceed.SourceItem.IsActual;
            apartment.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            apartment.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            apartment.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveApartmentParametersPortion(List<DistributionReader.Models.ClassifierData.ApartmentParameter> currentPortion)
    {
        var existsApartmentParameters = await _classifierDataRepository
            .GetApartmentParameters(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsApartmentParameters.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            ApartmentParameter fiasApartmentParameter;
            if (itemToProceed.DatabaseItem == null)
            {
                fiasApartmentParameter = new ApartmentParameter();
                fiasApartmentParameter.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddApartmentParameter(fiasApartmentParameter);
            }
            else
            {
                fiasApartmentParameter = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateApartmentParameter(fiasApartmentParameter);
            }
            
            fiasApartmentParameter.ObjectId = itemToProceed.SourceItem.ObjectId;
            fiasApartmentParameter.ChangeId = itemToProceed.SourceItem.ChangeId;
            fiasApartmentParameter.ChangeIdEnd = itemToProceed.SourceItem.ChangeIdEnd;
            fiasApartmentParameter.TypeId = itemToProceed.SourceItem.TypeId;
            fiasApartmentParameter.Value = itemToProceed.SourceItem.Value;
            fiasApartmentParameter.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            fiasApartmentParameter.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            fiasApartmentParameter.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveCarPlacesPortion(List<DistributionReader.Models.ClassifierData.CarPlace> currentPortion)
    {
        var existsCarPlaces = await _classifierDataRepository
            .GetCarPlaces(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsCarPlaces.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            CarPlace carPlace;
            if (itemToProceed.DatabaseItem == null)
            {
                carPlace = new CarPlace();
                carPlace.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddCarPlace(carPlace);
            }
            else
            {
                carPlace = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateCarPlace(carPlace);
            }
            
            carPlace.ObjectId = itemToProceed.SourceItem.ObjectId;
            carPlace.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            carPlace.ChangeId = itemToProceed.SourceItem.ChangeId;
            carPlace.Number = itemToProceed.SourceItem.Number;
            carPlace.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
            carPlace.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            carPlace.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            carPlace.IsActive = itemToProceed.SourceItem.IsActive;
            carPlace.IsActual = itemToProceed.SourceItem.IsActual;
            carPlace.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            carPlace.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            carPlace.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveCarPlaceParametersPortion(List<DistributionReader.Models.ClassifierData.CarPlaceParameter> currentPortion)
    {
        var existsCarPlaceParameters = await _classifierDataRepository
            .GetCarPlaceParameters(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsCarPlaceParameters.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            CarPlaceParameter fiasCarPlaceParameter;
            if (itemToProceed.DatabaseItem == null)
            {
                fiasCarPlaceParameter = new CarPlaceParameter();
                fiasCarPlaceParameter.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddCarPlaceParameter(fiasCarPlaceParameter);
            }
            else
            {
                fiasCarPlaceParameter = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateCarPlaceParameter(fiasCarPlaceParameter);
            }
            
            fiasCarPlaceParameter.ObjectId = itemToProceed.SourceItem.ObjectId;
            fiasCarPlaceParameter.ChangeId = itemToProceed.SourceItem.ChangeId;
            fiasCarPlaceParameter.ChangeIdEnd = itemToProceed.SourceItem.ChangeIdEnd;
            fiasCarPlaceParameter.TypeId = itemToProceed.SourceItem.TypeId;
            fiasCarPlaceParameter.Value = itemToProceed.SourceItem.Value;
            fiasCarPlaceParameter.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            fiasCarPlaceParameter.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            fiasCarPlaceParameter.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveHousesPortion(List<DistributionReader.Models.ClassifierData.House> currentPortion)
    {
        var existsHouses = await _classifierDataRepository
            .GetHouses(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsHouses.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            House house;
            if (itemToProceed.DatabaseItem == null)
            {
                house = new House();
                house.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddHouse(house);
            }
            else
            {
                house = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateHouse(house);
            }
            
            house.ObjectId = itemToProceed.SourceItem.ObjectId;
            house.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            house.ChangeId = itemToProceed.SourceItem.ChangeId;
            house.HouseNumber = itemToProceed.SourceItem.HouseNumber;
            house.AddedHouseNumber1 = itemToProceed.SourceItem.AddedHouseNumber1;
            house.AddedHouseNumber2 = itemToProceed.SourceItem.AddedHouseNumber2;
            house.HouseTypeId = itemToProceed.SourceItem.HouseTypeId == 0 ? null : itemToProceed.SourceItem.HouseTypeId;
            house.AddedHouseTypeId1 = itemToProceed.SourceItem.AddedHouseTypeId1 == 0 ? null : itemToProceed.SourceItem.AddedHouseTypeId1;
            house.AddedHouseTypeId2 = itemToProceed.SourceItem.AddedHouseTypeId2 == 0 ? null : itemToProceed.SourceItem.AddedHouseTypeId2;
            house.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
            house.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            house.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            house.IsActive = itemToProceed.SourceItem.IsActive;
            house.IsActual = itemToProceed.SourceItem.IsActual;
            house.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            house.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            house.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveHouseParametersPortion(List<DistributionReader.Models.ClassifierData.HouseParameter> currentPortion)
    {
        var existsHouseParameters = await _classifierDataRepository
            .GetHouseParameters(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsHouseParameters.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            HouseParameter fiasHouseParameter;
            if (itemToProceed.DatabaseItem == null)
            {
                fiasHouseParameter = new HouseParameter();
                fiasHouseParameter.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddHouseParameter(fiasHouseParameter);
            }
            else
            {
                fiasHouseParameter = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateHouseParameter(fiasHouseParameter);
            }
            
            fiasHouseParameter.ObjectId = itemToProceed.SourceItem.ObjectId;
            fiasHouseParameter.ChangeId = itemToProceed.SourceItem.ChangeId;
            fiasHouseParameter.ChangeIdEnd = itemToProceed.SourceItem.ChangeIdEnd;
            fiasHouseParameter.TypeId = itemToProceed.SourceItem.TypeId;
            fiasHouseParameter.Value = itemToProceed.SourceItem.Value;
            fiasHouseParameter.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            fiasHouseParameter.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            fiasHouseParameter.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveRoomsPortion(List<DistributionReader.Models.ClassifierData.Room> currentPortion)
    {
        var existsRooms = await _classifierDataRepository
            .GetRooms(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsRooms.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            Room room;
            if (itemToProceed.DatabaseItem == null)
            {
                room = new Room();
                room.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddRoom(room);
            }
            else
            {
                room = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateRoom(room);
            }
            
            room.ObjectId = itemToProceed.SourceItem.ObjectId;
            room.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            room.ChangeId = itemToProceed.SourceItem.ChangeId;
            room.RoomNumber = itemToProceed.SourceItem.RoomNumber;
            room.RoomTypeId = itemToProceed.SourceItem.RoomTypeId;
            room.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
            room.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            room.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            room.IsActive = itemToProceed.SourceItem.IsActive;
            room.IsActual = itemToProceed.SourceItem.IsActual;
            room.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            room.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            room.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveRoomParametersPortion(List<DistributionReader.Models.ClassifierData.RoomParameter> currentPortion)
    {
        var existsRoomParameters = await _classifierDataRepository
            .GetRoomParameters(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsRoomParameters.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            RoomParameter fiasHouseParameter;
            if (itemToProceed.DatabaseItem == null)
            {
                fiasHouseParameter = new RoomParameter();
                fiasHouseParameter.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddRoomParameter(fiasHouseParameter);
            }
            else
            {
                fiasHouseParameter = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateRoomParameter(fiasHouseParameter);
            }
            
            fiasHouseParameter.ObjectId = itemToProceed.SourceItem.ObjectId;
            fiasHouseParameter.ChangeId = itemToProceed.SourceItem.ChangeId;
            fiasHouseParameter.ChangeIdEnd = itemToProceed.SourceItem.ChangeIdEnd;
            fiasHouseParameter.TypeId = itemToProceed.SourceItem.TypeId;
            fiasHouseParameter.Value = itemToProceed.SourceItem.Value;
            fiasHouseParameter.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            fiasHouseParameter.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            fiasHouseParameter.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveSteadsPortion(List<DistributionReader.Models.ClassifierData.Stead> currentPortion)
    {
        var existsSteads = await _classifierDataRepository
            .GetSteads(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsSteads.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            Stead stead;
            if (itemToProceed.DatabaseItem == null)
            {
                stead = new Stead();
                stead.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddStead(stead);
            }
            else
            {
                stead = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateStead(stead);
            }
            
            stead.ObjectId = itemToProceed.SourceItem.ObjectId;
            stead.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            stead.ChangeId = itemToProceed.SourceItem.ChangeId;
            stead.Number = itemToProceed.SourceItem.Number;
            stead.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
            stead.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            stead.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            stead.IsActive = itemToProceed.SourceItem.IsActive;
            stead.IsActual = itemToProceed.SourceItem.IsActual;
            stead.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            stead.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            stead.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
    
    private async Task SaveSteadParametersPortion(List<DistributionReader.Models.ClassifierData.SteadParameter> currentPortion)
    {
        var existsSteadParameters = await _classifierDataRepository
            .GetSteadParameters(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsSteadParameters.AsQueryable(),
                o => o.Id,
                i => i.Id,
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();

        foreach (var itemToProceed in itemsToProceed)
        {
            SteadParameter fiasSteadParameter;
            if (itemToProceed.DatabaseItem == null)
            {
                fiasSteadParameter = new SteadParameter();
                fiasSteadParameter.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddSteadParameter(fiasSteadParameter);
            }
            else
            {
                fiasSteadParameter = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateSteadParameter(fiasSteadParameter);
            }
            
            fiasSteadParameter.ObjectId = itemToProceed.SourceItem.ObjectId;
            fiasSteadParameter.ChangeId = itemToProceed.SourceItem.ChangeId;
            fiasSteadParameter.ChangeIdEnd = itemToProceed.SourceItem.ChangeIdEnd;
            fiasSteadParameter.TypeId = itemToProceed.SourceItem.TypeId;
            fiasSteadParameter.Value = itemToProceed.SourceItem.Value;
            fiasSteadParameter.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            fiasSteadParameter.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            fiasSteadParameter.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveAsync();
        _classifierDataRepository.ClearChangeTracking();
        currentPortion.Clear();
    }
}