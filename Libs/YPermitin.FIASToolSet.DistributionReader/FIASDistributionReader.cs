using YPermitin.FIASToolSet.DistributionReader.DataCollections;
using YPermitin.FIASToolSet.DistributionReader.Exceptions;

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
        }}
    };
    private class CollectionInfo
    {
        public string FileSearchPattern;
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
    
    private T GetInternalCollection<T>()
    {
        if (_collectionsInfoByType.TryGetValue(typeof(T), out CollectionInfo collectionInfo))
        {
            string dataFile;
            var foundFiles = Directory.GetFiles(_workingDirectory, collectionInfo.FileSearchPattern);
            if (foundFiles.Length == 1)
            {
                FileInfo foundFileInfo = new FileInfo(foundFiles[0]);
                dataFile = Path.Combine(
                    _workingDirectory,
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