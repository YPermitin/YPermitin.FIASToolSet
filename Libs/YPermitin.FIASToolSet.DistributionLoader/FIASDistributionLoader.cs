using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.DistributionLoader.Exceptions;
using YPermitin.FIASToolSet.DistributionLoader.Extensions;
using YPermitin.FIASToolSet.DistributionLoader.Models;
using YPermitin.FIASToolSet.DistributionReader;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;
using YPermitin.FIASToolSet.DistributionReader.Exceptions;
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
                $"Не найден регион с кодом \"{region.Code}\" среди доступных регионов в дистрибутиве ФИАС.",
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

        bool emptyItemWasLoaded = false;
        AddressObjectTypeCollection fiasAddressObjectTypes;
        try
        {
            fiasAddressObjectTypes = fiasDistributionReader.GetAddressObjectTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (fiasAddressObjectType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasAddressObjectTypesFromDatabase = await _fiasBaseCatalogsRepository.GetAddressObjectTypes();
        foreach (var fiasAddressObjectTypeFromDatabase in fiasAddressObjectTypesFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasAddressObjectTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasAddressObjectTypes.All(e => e.Id != fiasAddressObjectTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveAddressObjectType(fiasAddressObjectTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            AddressObjectType emptyItem = await _fiasBaseCatalogsRepository.GetAddressObjectType(0);
            if (emptyItem == null)
            {
                emptyItem = new AddressObjectType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddAddressObjectType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateAddressObjectType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.Description = "Не указан";
            emptyItem.ShortName = "Не указан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
            emptyItem.Level = 0;
        }

        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadApartmentTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        bool emptyItemWasLoaded = false;
        ApartmentTypeCollection fiasApartmentTypes;
        try
        {
            fiasApartmentTypes = fiasDistributionReader.GetApartmentTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

            if (apartmentType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasApartmentTypesFromDatabase = await _fiasBaseCatalogsRepository.GetApartmentTypes();
        foreach (var fiasApartmentTypeFromDatabase in fiasApartmentTypesFromDatabase)
        {
            if(fiasApartmentTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasApartmentTypes.All(e => e.Id != fiasApartmentTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveApartmentType(fiasApartmentTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            ApartmentType emptyItem = await _fiasBaseCatalogsRepository.GetApartmentType(0);
            if (emptyItem == null)
            {
                emptyItem = new ApartmentType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddApartmentType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateApartmentType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.Description = "Не указан";
            emptyItem.ShortName = "Не указан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
            emptyItem.IsActive = true;
        }

        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadHouseTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        bool emptyItemWasLoaded = false;
        HouseTypeCollection fiasHouseTypes;
        try
        {
            fiasHouseTypes = fiasDistributionReader.GetHouseTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (houseType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasHouseTypesFromDatabase = await _fiasBaseCatalogsRepository.GetHouseTypes();
        foreach (var fiasHouseTypeFromDatabase in fiasHouseTypesFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasHouseTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasHouseTypes.All(e => e.Id != fiasHouseTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveHouseType(fiasHouseTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            HouseType emptyItem = await _fiasBaseCatalogsRepository.GetHouseType(0);
            if (emptyItem == null)
            {
                emptyItem = new HouseType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddHouseType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateHouseType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.Description = "Не указан";
            emptyItem.ShortName = "Не указан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
            emptyItem.IsActive = true;
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadNormativeDocKinds()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        bool emptyItemWasLoaded = false;
        NormativeDocKindCollection fiasNormativeDocKinds;
        try
        {
            fiasNormativeDocKinds = fiasDistributionReader.GetNormativeDocKinds();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (normativeDocKind.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasNormativeDocKindsFromDatabase = await _fiasBaseCatalogsRepository.GetNormativeDocKinds();
        foreach (var fiasNormativeDocKindFromDatabase in fiasNormativeDocKindsFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasNormativeDocKindFromDatabase.Id == 0)
                continue;
            
            if (fiasNormativeDocKinds.All(e => e.Id != fiasNormativeDocKindFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveNormativeDocKind(fiasNormativeDocKindFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            NormativeDocKind emptyItem = await _fiasBaseCatalogsRepository.GetNormativeDocKind(0);
            if (emptyItem == null)
            {
                emptyItem = new NormativeDocKind();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddNormativeDocKind(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateNormativeDocKind(emptyItem);
            }

            emptyItem.Name = "Не указан";
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadNormativeDocTypes()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        bool emptyItemWasLoaded = false;
        NormativeDocTypeCollection fiasNormativeDocTypes;
        try
        {
            fiasNormativeDocTypes = fiasDistributionReader.GetNormativeDocTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (normativeDocType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasNormativeDocTypesFromDatabase = await _fiasBaseCatalogsRepository.GetNormativeDocTypes();
        foreach (var fiasNormativeDocTypeFromDatabase in fiasNormativeDocTypesFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasNormativeDocTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasNormativeDocTypes.All(e => e.Id != fiasNormativeDocTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveNormativeDocType(fiasNormativeDocTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            NormativeDocType emptyItem = await _fiasBaseCatalogsRepository.GetNormativeDocType(0);
            if (emptyItem == null)
            {
                emptyItem = new NormativeDocType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddNormativeDocType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateNormativeDocType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadObjectLevels()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        bool emptyItemWasLoaded = false;
        ObjectLevelCollection fiasObjectLevels;
        try
        {
            fiasObjectLevels = fiasDistributionReader.GetObjectLevels();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (objectLevel.Level == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasObjectLevelsFromDatabase = await _fiasBaseCatalogsRepository.GetObjectLevels();
        foreach (var fiasObjectLevelFromDatabase in fiasObjectLevelsFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasObjectLevelFromDatabase.Level == 0)
                continue;
            
            if (fiasObjectLevels.All(e => e.Level != fiasObjectLevelFromDatabase.Level))
            {
                _fiasBaseCatalogsRepository.RemoveObjectLevel(fiasObjectLevelFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            ObjectLevel emptyItem = await _fiasBaseCatalogsRepository.GetObjectLevel(0);
            if (emptyItem == null)
            {
                emptyItem = new ObjectLevel();
                emptyItem.Level = 0;
                _fiasBaseCatalogsRepository.AddObjectLevel(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateObjectLevel(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.IsActive = true;
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadOperationTypes()
    {
        var fiasDistributionReader = GetDistributionReader();
        
        bool emptyItemWasLoaded = false;
        OperationTypeCollection fiasOperationTypes;
        try
        {
            fiasOperationTypes = fiasDistributionReader.GetOperationTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (operationType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasOperationTypesFromDatabase = await _fiasBaseCatalogsRepository.GetOperationTypes();
        foreach (var fiasOperationTypeFromDatabase in fiasOperationTypesFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasOperationTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasOperationTypes.All(e => e.Id != fiasOperationTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveOperationType(fiasOperationTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            OperationType emptyItem = await _fiasBaseCatalogsRepository.GetOperationType(0);
            if (emptyItem == null)
            {
                emptyItem = new OperationType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddOperationType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateOperationType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
            emptyItem.IsActive = true;
        }
                
        await _fiasBaseCatalogsRepository.SaveAsync();
    }

    public async Task LoadParameterTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        bool emptyItemWasLoaded = false;
        ParameterTypeCollection fiasParameterTypes;
        try
        {
            fiasParameterTypes = fiasDistributionReader.GetParameterTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (parameterType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasParameterTypesFromDatabase = await _fiasBaseCatalogsRepository.GetParameterTypes();
        foreach (var fiasParameterTypeFromDatabase in fiasParameterTypesFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasParameterTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasParameterTypes.All(e => e.Id != fiasParameterTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveParameterType(fiasParameterTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            ParameterType emptyItem = await _fiasBaseCatalogsRepository.GetParameterType(0);
            if (emptyItem == null)
            {
                emptyItem = new ParameterType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddParameterType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateParameterType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.Description = "Не указан";
            emptyItem.Code = "НеУказан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
            emptyItem.IsActive = true;
        }

        await _fiasBaseCatalogsRepository.SaveAsync();
    }
    
    public async Task LoadRoomTypes()
    {
        var fiasDistributionReader = GetDistributionReader();

        bool emptyItemWasLoaded = false;
        RoomTypeCollection fiasRoomTypes;
        try
        {
            fiasRoomTypes = fiasDistributionReader.GetRoomTypes();
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
            
            if (roomType.Id == 0)
            {
                emptyItemWasLoaded = true;
            }
        }

        var fiasRoomTypesFromDatabase = await _fiasBaseCatalogsRepository.GetRoomTypes();
        foreach (var fiasRoomTypeFromDatabase in fiasRoomTypesFromDatabase)
        {
            // Пустой элемент не удаляем
            if(fiasRoomTypeFromDatabase.Id == 0)
                continue;
            
            if (fiasRoomTypes.All(e => e.Id != fiasRoomTypeFromDatabase.Id))
            {
                _fiasBaseCatalogsRepository.RemoveRoomType(fiasRoomTypeFromDatabase);
            }
        }
        
        // Пустое значение, если оно не было загружено ранее
        if (!emptyItemWasLoaded)
        {
            RoomType emptyItem = await _fiasBaseCatalogsRepository.GetRoomType(0);
            if (emptyItem == null)
            {
                emptyItem = new RoomType();
                emptyItem.Id = 0;
                _fiasBaseCatalogsRepository.AddRoomType(emptyItem);
            }
            else
            {
                _fiasBaseCatalogsRepository.UpdateRoomType(emptyItem);
            }

            emptyItem.Name = "Не указан";
            emptyItem.Description = "Не указан";
            emptyItem.StartDate = new DateTime(1900, 1, 1);
            emptyItem.UpdateDate = new DateTime(1900, 1, 1);
            emptyItem.EndDate = new DateTime(1900, 1, 1);
            emptyItem.IsActive = true;
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

        AddressObjectCollection fiasAddressObjects;
        try
        {
            fiasAddressObjects = fiasDistributionReader.GetAddressObjects(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }

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

        AddressObjectDivisionCollection fiasAddressObjectDivisions;
        try
        {
            fiasAddressObjectDivisions = fiasDistributionReader.GetAddressObjectDivisions(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        AddressObjectParameterCollection fiasAddressObjectParameters;
        try
        {
            fiasAddressObjectParameters = fiasDistributionReader.GetAddressObjectParameters(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        AddressObjectAdmHierarchyCollection fiasAddressObjectsAdmHierarchy;
        try
        {
            fiasAddressObjectsAdmHierarchy = fiasDistributionReader.GetAddressObjectsAdmHierarchy(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
    /// Загрузка информации о иерархии муниципального деления адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о иерархии муниципального деления адресных объектов</param>
    public async Task LoadAddressObjectsMunHierarchy(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        MunHierarchyCollection fiasAddressObjectsMunHierarchy;
        try
        {
            fiasAddressObjectsMunHierarchy = fiasDistributionReader.GetMunHierarchy(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
        List<DistributionReader.Models.ClassifierData.MunHierarchy> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.MunHierarchy>();
        
        foreach (var fiasAddressObjectMunHierarchy in fiasAddressObjectsMunHierarchy)
        {
            currentPortion.Add(fiasAddressObjectMunHierarchy);

            if (currentPortion.Count == 1000)
            {
                await SaveAddressObjectsMunHierarchyPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveAddressObjectsMunHierarchyPortion(currentPortion);
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

        ApartmentCollection fiasApartments;
        try
        {
            fiasApartments = fiasDistributionReader.GetApartments(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        ApartmentParameterCollection fiasApartmentParameters;
        try
        {
            fiasApartmentParameters = fiasDistributionReader.GetApartmentParameters(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        CarPlaceCollection fiasCarPlaces;
        try
        {
            fiasCarPlaces = fiasDistributionReader.GetCarPlaces(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        CarPlaceParameterCollection fiasCarPlaceParameters;
        try
        {
            fiasCarPlaceParameters = fiasDistributionReader.GetCarPlaceParameters(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        HouseCollection fiasHouses;
        try
        {
            fiasHouses = fiasDistributionReader.GetHouses(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        HouseParameterCollection fiasHouseParameters;
        try
        {
            fiasHouseParameters = fiasDistributionReader.GetHouseParameters(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        RoomCollection fiasRooms;
        try
        {
            fiasRooms = fiasDistributionReader.GetRooms(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        RoomParameterCollection fiasRoomParameters;
        try
        {
            fiasRoomParameters = fiasDistributionReader.GetRoomParameters(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        SteadCollection fiasSteads;
        try
        {
            fiasSteads = fiasDistributionReader.GetSteads(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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

        SteadParameterCollection fiasSteadParameters;
        try
        {
            fiasSteadParameters = fiasDistributionReader.GetSteadParameters(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
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
    
    /// <summary>
    /// Загрузка информации о нормативных документах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о нормативных документах</param>
    public async Task LoadNormativeDocuments(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        NormativeDocumentCollection fiasNormativeDocuments;
        try
        {
            fiasNormativeDocuments = fiasDistributionReader.GetNormativeDocuments(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
        List<DistributionReader.Models.ClassifierData.NormativeDocument> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.NormativeDocument>();
        
        foreach (var fiasNormativeDocument in fiasNormativeDocuments)
        {
            currentPortion.Add(fiasNormativeDocument);

            if (currentPortion.Count == 1000)
            {
                await SaveNormativeDocumentsPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveNormativeDocumentsPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка истории изменений адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о истории изменений адресных объектов</param>
    public async Task LoadChangeHistory(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        ChangeHistoryCollection fiasChangeHistory;
        try
        {
            fiasChangeHistory = fiasDistributionReader.GetChangeHistory(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
        List<DistributionReader.Models.ClassifierData.ChangeHistory> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.ChangeHistory>();
        
        foreach (var fiasChangeHistoryItem in fiasChangeHistory)
        {
            currentPortion.Add(fiasChangeHistoryItem);

            if (currentPortion.Count == 1000)
            {
                await SaveChangeHistoryPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveChangeHistoryPortion(currentPortion);
        }
    }
    
    /// <summary>
    /// Загрузка реестра адресных элементов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о реестре адресных элементов</param>
    public async Task LoadObjectsRegistry(Region region)
    {
        var fiasDistributionReader = GetDistributionReader();
        var fiasDistributionRegion = fiasDistributionReader
            .GetRegions()
            .FirstOrDefault(e => e.Code == region.Code);
        if (fiasDistributionRegion == null)
        {
            throw new RegionNotFoundException("Не удалось найти регион.", region.Code.ToString());
        }

        ObjectRegistryCollection fiasObjectsRegistry;
        try
        {
            fiasObjectsRegistry = fiasDistributionReader.GetObjectsRegistry(fiasDistributionRegion);
        }
        catch (FIASDataNotFoundException)
        {
            return;
        }
        
        List<DistributionReader.Models.ClassifierData.ObjectRegistry> currentPortion = 
            new List<DistributionReader.Models.ClassifierData.ObjectRegistry>();
        
        foreach (var fiasObjectRegistry in fiasObjectsRegistry)
        {
            currentPortion.Add(fiasObjectRegistry);

            if (currentPortion.Count == 1000)
            {
                await SaveObjectsRegistryPortion(currentPortion);
            }
        }

        if (currentPortion.Count > 0)
        {
            await SaveObjectsRegistryPortion(currentPortion);
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
        currentPortion.Clear();
    }
    
    private async Task SaveAddressObjectsMunHierarchyPortion(List<DistributionReader.Models.ClassifierData.MunHierarchy> currentPortion)
    {
        DistributionReader.Models.ClassifierData.MunHierarchy fiasAddressObjectMunHierarchy;
        var existsAddressObjectsMunHierarchy = await _classifierDataRepository
            .GetAddressObjectsMunHierarchy(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsAddressObjectsMunHierarchy.AsQueryable(),
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
            MunHierarchy addressObjectMunHierarchy;
            if (itemToProceed.DatabaseItem == null)
            {
                addressObjectMunHierarchy = new MunHierarchy();
                addressObjectMunHierarchy.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddAddressObjectMunHierarchy(addressObjectMunHierarchy);
            }
            else
            {
                addressObjectMunHierarchy = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateAddressObjectMunHierarchy(addressObjectMunHierarchy);
            }
            
            addressObjectMunHierarchy.ObjectId = itemToProceed.SourceItem.ObjectId;
            addressObjectMunHierarchy.ChangeId = itemToProceed.SourceItem.ChangeId;
            addressObjectMunHierarchy.ParentObjectId = itemToProceed.SourceItem.ParentObjectId;
            addressObjectMunHierarchy.OKTMO = itemToProceed.SourceItem.OKTMO;
            addressObjectMunHierarchy.PreviousAddressObjectId = itemToProceed.SourceItem.PreviousAddressObjectId;
            addressObjectMunHierarchy.NextAddressObjectId = itemToProceed.SourceItem.NextAddressObjectId;
            addressObjectMunHierarchy.IsActive = itemToProceed.SourceItem.IsActive;
            addressObjectMunHierarchy.Path = itemToProceed.SourceItem.Path;
            addressObjectMunHierarchy.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            addressObjectMunHierarchy.StartDate = itemToProceed.SourceItem.StartDate.ToDateTime(TimeOnly.MinValue);
            addressObjectMunHierarchy.EndDate = itemToProceed.SourceItem.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
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

        await _classifierDataRepository.SaveBulkAsync();
        currentPortion.Clear();
    }
    
    private async Task SaveNormativeDocumentsPortion(List<DistributionReader.Models.ClassifierData.NormativeDocument> currentPortion)
    {
        var existsNormativeDocuments = await _classifierDataRepository
            .GetNormativeDocuments(ids: currentPortion.Select(e => e.Id).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsNormativeDocuments.AsQueryable(),
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
            NormativeDocument normativeDocument;
            if (itemToProceed.DatabaseItem == null)
            {
                normativeDocument = new NormativeDocument();
                normativeDocument.Id = itemToProceed.SourceItem.Id;
                _classifierDataRepository.AddNormativeDocument(normativeDocument);
            }
            else
            {
                normativeDocument = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateNormativeDocument(normativeDocument);
            }
            
            normativeDocument.Name = itemToProceed.SourceItem.Name;
            normativeDocument.Date = itemToProceed.SourceItem.Date.ToDateTime(TimeOnly.MinValue);
            normativeDocument.Number = itemToProceed.SourceItem.Number;
            normativeDocument.TypeId = itemToProceed.SourceItem.TypeId;
            normativeDocument.KindId = itemToProceed.SourceItem.KindId;
            normativeDocument.OrgName = itemToProceed.SourceItem.OrgName;
            normativeDocument.RegNumber = itemToProceed.SourceItem.RegNumber;
            normativeDocument.RegDate = itemToProceed.SourceItem.RegDate.ToDateTime(TimeOnly.MinValue);
            normativeDocument.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
            normativeDocument.AccDate = itemToProceed.SourceItem.AccDate.ToDateTime(TimeOnly.MinValue);
            normativeDocument.Comment = itemToProceed.SourceItem.Comment;
        }

        await _classifierDataRepository.SaveBulkAsync();
        currentPortion.Clear();
    }
    
    private async Task SaveChangeHistoryPortion(List<DistributionReader.Models.ClassifierData.ChangeHistory> currentPortion)
    {
        var existsNormativeDocuments = await _classifierDataRepository
            .GetChangeHistoryItems(keys: currentPortion.Select(e => new ChangeHistory.ChangeHistoryItemKey()
            {
                ChangeId = e.ChangeId,
                AddressObjectGuid = e.AddressObjectGuid,
                ObjectId = e.ObjectId
            }).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsNormativeDocuments.AsQueryable(),
                o => new { o.ObjectId, o.AddressObjectGuid, o.ChangeId },
                i => new { i.ObjectId, i.AddressObjectGuid, i.ChangeId },
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();
        
        foreach (var itemToProceed in itemsToProceed)
        {
            ChangeHistory changeHistory;
            if (itemToProceed.DatabaseItem == null)
            {
                changeHistory = new ChangeHistory();
                changeHistory.ObjectId = itemToProceed.SourceItem.ObjectId;
                changeHistory.AddressObjectGuid = itemToProceed.SourceItem.AddressObjectGuid;
                changeHistory.ChangeId = itemToProceed.SourceItem.ChangeId;
                _classifierDataRepository.AddChangeHistory(changeHistory);
            }
            else
            {
                changeHistory = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateChangeHistory(changeHistory);
            }
            
            changeHistory.ObjectId = itemToProceed.SourceItem.ObjectId;
            changeHistory.AddressObjectGuid = itemToProceed.SourceItem.AddressObjectGuid;
            changeHistory.ChangeId = itemToProceed.SourceItem.ChangeId;
            changeHistory.ChangeDate = itemToProceed.SourceItem.ChangeDate.ToDateTime(TimeOnly.MinValue);
            changeHistory.NormativeDocId = itemToProceed.SourceItem.NormativeDocId == 0 ? null : itemToProceed.SourceItem.NormativeDocId;
            changeHistory.OperationTypeId = itemToProceed.SourceItem.OperationTypeId;
        }

        await _classifierDataRepository.SaveBulkAsync();
        currentPortion.Clear();
    }
    
    private async Task SaveObjectsRegistryPortion(List<DistributionReader.Models.ClassifierData.ObjectRegistry> currentPortion)
    {
        var existsObjectsRegistry = await _classifierDataRepository
            .GetObjectRegistryItems(keys: currentPortion.Select(e => new ObjectRegistry.ObjectRegistryItemKey()
            {
                ChangeId = e.ChangeId,
                ObjectGuid = e.ObjectGuid,
                ObjectId = e.ObjectId
            }).ToList());

        var itemsToProceed = currentPortion.AsQueryable()
            .LeftJoin(existsObjectsRegistry.AsQueryable(),
                o => new { o.ObjectId, o.ObjectGuid, o.ChangeId },
                i => new { i.ObjectId, i.ObjectGuid, i.ChangeId },
                (r) => new
                {
                    SourceItem = r.Outer,
                    DatabaseItem = r.Inner
                })
            .ToList();
        
        foreach (var itemToProceed in itemsToProceed)
        {
            ObjectRegistry objectRegistry;
            if (itemToProceed.DatabaseItem == null)
            {
                objectRegistry = new ObjectRegistry();
                objectRegistry.ObjectId = itemToProceed.SourceItem.ObjectId;
                objectRegistry.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
                objectRegistry.ChangeId = itemToProceed.SourceItem.ChangeId;
                _classifierDataRepository.AddObjectRegistry(objectRegistry);
            }
            else
            {
                objectRegistry = itemToProceed.DatabaseItem;
                _classifierDataRepository.UpdateObjectRegistry(objectRegistry);
            }
            
            objectRegistry.ObjectId = itemToProceed.SourceItem.ObjectId;
            objectRegistry.ObjectGuid = itemToProceed.SourceItem.ObjectGuid;
            objectRegistry.ChangeId = itemToProceed.SourceItem.ChangeId;
            objectRegistry.IsActive = itemToProceed.SourceItem.IsActive;
            objectRegistry.LevelId = itemToProceed.SourceItem.LevelId;
            objectRegistry.CreateDate = itemToProceed.SourceItem.CreateDate.ToDateTime(TimeOnly.MinValue);
            objectRegistry.UpdateDate = itemToProceed.SourceItem.UpdateDate.ToDateTime(TimeOnly.MinValue);
        }

        await _classifierDataRepository.SaveBulkAsync();
        currentPortion.Clear();
    }
}