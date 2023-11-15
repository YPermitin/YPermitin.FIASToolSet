using System.Globalization;
using System.Text.RegularExpressions;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;
using YPermitin.FIASToolSet.DistributionReader.Exceptions;
using YPermitin.FIASToolSet.DistributionReader.Models;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader;

public class FIASDistributionReader : IFIASDistributionReader
{
    private readonly string _workingDirectory;

    #region CollectionInfo

    private readonly Dictionary<Type, CollectionInfo> _collectionsInfoByType = new()
    {
        { typeof(NormativeDocKindCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_NORMATIVE_DOCS_KINDS_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о видах нормативных документов."
        }},
        { typeof(NormativeDocTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_NORMATIVE_DOCS_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах нормативных документов."
        }},
        { typeof(ObjectLevelCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_OBJECT_LEVELS*.XML",
            ErrorMessage = "Не обнаружен файл с информацией об уровнях адресных объектов."
        }},
        { typeof(RoomTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_ROOM_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах помещений."
        }},
        { typeof(ApartmentTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_APARTMENT_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах квартир."
        }},
        { typeof(HouseTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_HOUSE_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах строений."
        }},
        { typeof(OperationTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_OPERATION_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах операций."
        }},
        { typeof(AddressObjectTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_ADDR_OBJ_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах адресных объектов."
        }},
        { typeof(ParameterTypeCollection), new CollectionInfo()
        {
            FileSearchPattern = "AS_PARAM_TYPES_*.XML",
            ErrorMessage = "Не обнаружен файл с информацией о типах параметров."
        }},
        { typeof(AddressObjectCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_ADDR_OBJ_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией об адресных объектах."
        }},
        { typeof(AddressObjectDivisionCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_ADDR_OBJ_DIVISION_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о переподчинении адресных объектах."
        }},
        { typeof(AddressObjectParameterCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_ADDR_OBJ_PARAMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о параметрах адресных объектах."
        }},
        { typeof(AddressObjectAdmHierarchyCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_ADM_HIERARCHY_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией по иерархии в административном делении."
        }},
        { typeof(ApartmentCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_APARTMENTS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о помещениях."
        }},
        { typeof(ApartmentParameterCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_APARTMENTS_PARAMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о параметрах помещений."
        }},
        { typeof(CarPlaceCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_CARPLACES_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о машино-местах."
        }},
        { typeof(CarPlaceParameterCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_CARPLACES_PARAMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о параметрах машино-мест."
        }},
        { typeof(ChangeHistoryCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_CHANGE_HISTORY_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией об истории изменения адресных элементов."
        }},
        { typeof(HouseCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_HOUSES_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о строениях."
        }},
        { typeof(HouseParameterCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_HOUSES_PARAMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о параметрах строения."
        }},
        { typeof(MunHierarchyCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_MUN_HIERARCHY_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о иерархии в муниципальном делении."
        }},
        { typeof(NormativeDocumentCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_NORMATIVE_DOCS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о нормативных документах."
        }},
        { typeof(ObjectRegistryCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_REESTR_OBJECTS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о реестре адресных элементов."
        }},
        { typeof(RoomCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_ROOMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о комнатах."
        }},
        { typeof(RoomParameterCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_ROOMS_PARAMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о параметрах комнат."
        }},
        { typeof(SteadCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_STEADS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о земельных участках."
        }},
        { typeof(SteadParameterCollection), new CollectionInfo()
        {
            FileSearchPatternRegex = "AS_STEADS_PARAMS_\\d\\d\\d\\d\\d\\d\\d\\d*.+XML",
            ErrorMessage = "Не обнаружен файл с информацией о параметрах земельных участков."
        }}
    };
    private class CollectionInfo
    {
        public string FileSearchPattern;
        public string FileSearchPatternRegex;
        public string ErrorMessage;
    }
    
    #endregion

    public FIASDistributionReader(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
    }

    /// <summary>
    /// Получение версии дистрибутива ФИАС в строковм виде
    /// </summary>
    /// <returns>Строка с информацией о версии ФИАС</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public string GetVersionAsString()
    {
        string fileVersionInfoPath = Path.Combine(
            _workingDirectory,
            "version.txt"
        );
        
        string version;

        if (File.Exists(fileVersionInfoPath))
        {
            version = File.ReadAllText(fileVersionInfoPath).Trim();
        }
        else
        {
            throw new FIASDataNotFoundException("Не обнаружен файл с информацией о версии.", fileVersionInfoPath);
        }

        return version;
    }

    /// <summary>
    /// Получение версии дистрибутива ФИАС в числовом виде
    /// </summary>
    /// <returns>Версия ФИАС числом</returns>
    /// <exception cref="FIASBadDataException">Не удалось преобразовать версию к числовому формату</exception>
    public int GetVersion()
    {
        string versionAsString = GetVersionAsString();

        if (int.TryParse(versionAsString.Replace(".", string.Empty), out int versionAsInt))
        {
            return versionAsInt;
        }
        else
        {
            var exceptionObject = new FIASBadDataException(
                $"Не удалось конвертировать значение версии \"{versionAsString}\" в числовой формат.");
            exceptionObject.Data.Add("VersionAsString", versionAsString);
            throw exceptionObject;
        }
    }

    public List<Region> GetRegions()
    {
        var allRegions = new List<Region>();
        
        var regionDirectories = Directory.GetDirectories(
            _workingDirectory, 
            "*", 
            SearchOption.TopDirectoryOnly);
        foreach (var regionDirectory in regionDirectories)
        {
            var regionDirectoryInfo = new DirectoryInfo(regionDirectory);
            if (int.TryParse(regionDirectoryInfo.Name, NumberStyles.Number, CultureInfo.InvariantCulture,
                    out int regionCode))
            {
                allRegions.Add(new Region(regionCode));
            }
        }

        return allRegions;
    }
    
    #region BaseCatalogs
    
    /// <summary>
    /// Получение коллекции видов нормативных документов
    /// </summary>
    /// <returns>Коллекция видов нормативных документов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public NormativeDocKindCollection GetNormativeDocKinds()
    {
        return GetInternalCollection<NormativeDocKindCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов нормативных документов
    /// </summary>
    /// <returns>Коллекция типов нормативных документов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public NormativeDocTypeCollection GetNormativeDocTypes()
    {
        return GetInternalCollection<NormativeDocTypeCollection>();
    }
    
    /// <summary>
    /// Получение коллекции уровней адресных объектов 
    /// </summary>
    /// <returns>Коллекция уровней адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ObjectLevelCollection GetObjectLevels()
    {
        return GetInternalCollection<ObjectLevelCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов помещений
    /// </summary>
    /// <returns>Коллекция типов помещений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public RoomTypeCollection GetRoomTypes()
    {
        return GetInternalCollection<RoomTypeCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов квартир
    /// </summary>
    /// <returns>Коллекция типов квартир</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ApartmentTypeCollection GetApartmentTypes()
    {
        return GetInternalCollection<ApartmentTypeCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов строений
    /// </summary>
    /// <returns>Коллекция типов строений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public HouseTypeCollection GetHouseTypes()
    {
        return GetInternalCollection<HouseTypeCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов операций
    /// </summary>
    /// <returns>Коллекция типов операций</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public OperationTypeCollection GetOperationTypes()
    {
        return GetInternalCollection<OperationTypeCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов адресных объектов
    /// </summary>
    /// <returns>Коллекция типов адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public AddressObjectTypeCollection GetAddressObjectTypes()
    {
        return GetInternalCollection<AddressObjectTypeCollection>();
    }
    
    /// <summary>
    /// Получение коллекции типов параметров
    /// </summary>
    /// <returns>Коллекция типов параметров</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ParameterTypeCollection GetParameterTypes()
    {
        return GetInternalCollection<ParameterTypeCollection>();
    }
    
    #endregion

    #region ClassifierData

    /// <summary>
    /// Получение коллекции адресных объектов
    /// </summary>
    /// <returns>Коллекция адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public AddressObjectCollection GetAddressObjects(Region region)
    {
        return GetInternalCollection<AddressObjectCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о переподчинении адресных объектов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о переподчинении адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public AddressObjectDivisionCollection GetAddressObjectDivisions(Region region)
    {
        return GetInternalCollection<AddressObjectDivisionCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах адресных объектов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public AddressObjectParameterCollection GetAddressObjectParameters(Region region)
    {
        return GetInternalCollection<AddressObjectParameterCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о иерарххии в административном делении
    /// </summary>
    /// <returns>Коллекция элементов с информацией о иерарххии в административном делении</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public AddressObjectAdmHierarchyCollection GetAddressObjectsAdmHierarchy(Region region)
    {
        return GetInternalCollection<AddressObjectAdmHierarchyCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о помещениях
    /// </summary>
    /// <returns>Коллекция элементов с информацией о помещениях</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ApartmentCollection GetApartments(Region region)
    {
        return GetInternalCollection<ApartmentCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах помещений
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах помещений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ApartmentParameterCollection GetApartmentParameters(Region region)
    {
        return GetInternalCollection<ApartmentParameterCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о машино-местах
    /// </summary>
    /// <returns>Коллекция элементов с информацией о машино-местах</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public CarPlaceCollection GetCarPlaces(Region region)
    {
        return GetInternalCollection<CarPlaceCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах машино-места
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах машино-места</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public CarPlaceParameterCollection GetCarPlaceParameters(Region region)
    {
        return GetInternalCollection<CarPlaceParameterCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией об истории изменений адресных элементов
    /// </summary>
    /// <returns>Коллекция элементов с информацией об истории изменений адресных элементов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ChangeHistoryCollection GetChangeHistory(Region region)
    {
        return GetInternalCollection<ChangeHistoryCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о строениях
    /// </summary>
    /// <returns>Коллекция элементов с информацией о строениях</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public HouseCollection GetHouses(Region region)
    {
        return GetInternalCollection<HouseCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах строений
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах строений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public HouseParameterCollection GetHouseParameters(Region region)
    {
        return GetInternalCollection<HouseParameterCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о иерарххии в муниципальном делении
    /// </summary>
    /// <returns>Коллекция элементов с информацией о иерарххии в муниципальном делении</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public MunHierarchyCollection GetMunHierarchy(Region region)
    {
        return GetInternalCollection<MunHierarchyCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о нормативных документов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о нормативных документов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public NormativeDocumentCollection GetNormativeDocuments(Region region)
    {
        return GetInternalCollection<NormativeDocumentCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о реестре адресных элементов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о реестре адресных элементов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public ObjectRegistryCollection GetObjectsRegistry(Region region)
    {
        return GetInternalCollection<ObjectRegistryCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о комнатах
    /// </summary>
    /// <returns>Коллекция элементов с информацией о комнатах</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public RoomCollection GetRooms(Region region)
    {
        return GetInternalCollection<RoomCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах комнат
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах комнат</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public RoomParameterCollection GetRoomParameters(Region region)
    {
        return GetInternalCollection<RoomParameterCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о земельных участках
    /// </summary>
    /// <returns>Коллекция элементов с информацией о земельных участках</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public SteadCollection GetSteads(Region region)
    {
        return GetInternalCollection<SteadCollection>(region);
    }
    
    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах земельных участков
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах земельных участков</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    public SteadParameterCollection GetSteadParameters(Region region)
    {
        return GetInternalCollection<SteadParameterCollection>(region);
    }
    
    #endregion
    
    private T GetInternalCollection<T>(Region region = null)
    {
        if (_collectionsInfoByType.TryGetValue(typeof(T), out CollectionInfo collectionInfo))
        {
            string workingDirectory;
            if (region == null)
                workingDirectory = _workingDirectory;
            else
                workingDirectory = Path.Combine(_workingDirectory, region.Code.ToString());
            
            string dataFile;
            string[] foundFiles;
            if(collectionInfo.FileSearchPattern != null)
                foundFiles = Directory.GetFiles(workingDirectory, collectionInfo.FileSearchPattern);
            else if (collectionInfo.FileSearchPatternRegex != null)
            {
                foundFiles = Directory.GetFiles(workingDirectory)
                    .Where(e =>
                    {
                        var fileDataInfo = new FileInfo(e);
                        return Regex.IsMatch(fileDataInfo.Name, collectionInfo.FileSearchPatternRegex);
                    })
                    .ToArray();
            }
            else
            {
                foundFiles = Array.Empty<string>();
            }
            if (foundFiles.Length == 1)
            {
                FileInfo foundFileInfo = new FileInfo(foundFiles[0]);
                dataFile = Path.Combine(
                    workingDirectory,
                    foundFileInfo.Name
                );
            }
            else
            {
                throw new FIASDataNotFoundException(
                    collectionInfo.ErrorMessage,
                    collectionInfo.FileSearchPattern);
            }

            return (T)Activator.CreateInstance(typeof(T), dataFile);
        }
        else
        {
            throw new InvalidOperationException($"Не удалось найти настройки для создания коллекции элементов по типу \"{typeof(T)}\"");
        }
    }
}