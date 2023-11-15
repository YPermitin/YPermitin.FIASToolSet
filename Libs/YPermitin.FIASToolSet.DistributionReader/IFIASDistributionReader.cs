using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;
using YPermitin.FIASToolSet.DistributionReader.Exceptions;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader;

public interface IFIASDistributionReader
{
    /// <summary>
    /// Получение версии дистрибутива ФИАС в строковм виде
    /// </summary>
    /// <returns>Строка с информацией о версии ФИАС</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    string GetVersionAsString();

    /// <summary>
    /// Получение версии дистрибутива ФИАС в числовом виде
    /// </summary>
    /// <returns>Версия ФИАС числом</returns>
    /// <exception cref="FIASBadDataException">Не удалось преобразовать версию к числовому формату</exception>
    int GetVersion();

    /// <summary>
    /// Получение списка доступных регионов в дистрибутиве ФИАС, доступных для работы
    /// </summary>
    /// <returns>Коллекция регионов классификатора ФИАС, доступных для работы</returns>
    List<Region> GetRegions();

    #region BaseCatalogs
    
    /// <summary>
    /// Получение коллекции видов нормативных документов
    /// </summary>
    /// <returns>Коллекция видов нормативных документов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    NormativeDocKindCollection GetNormativeDocKinds();

    /// <summary>
    /// Получение коллекции типов нормативных документов
    /// </summary>
    /// <returns>Коллекция типов нормативных документов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    NormativeDocTypeCollection GetNormativeDocTypes();

    /// <summary>
    /// Получение коллекции уровней адресных объектов 
    /// </summary>
    /// <returns>Коллекция уровней адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ObjectLevelCollection GetObjectLevels();

    /// <summary>
    /// Получение коллекции типов помещений
    /// </summary>
    /// <returns>Коллекция типов помещений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    RoomTypeCollection GetRoomTypes();

    /// <summary>
    /// Получение коллекции типов квартир
    /// </summary>
    /// <returns>Коллекция типов квартир</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ApartmentTypeCollection GetApartmentTypes();

    /// <summary>
    /// Получение коллекции типов строений
    /// </summary>
    /// <returns>Коллекция типов строений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    HouseTypeCollection GetHouseTypes();

    /// <summary>
    /// Получение коллекции типов операций
    /// </summary>
    /// <returns>Коллекция типов операций</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    OperationTypeCollection GetOperationTypes();

    /// <summary>
    /// Получение коллекции типов адресных объектов
    /// </summary>
    /// <returns>Коллекция типов адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    AddressObjectTypeCollection GetAddressObjectTypes();

    /// <summary>
    /// Получение коллекции типов параметров
    /// </summary>
    /// <returns>Коллекция типов параметров</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ParameterTypeCollection GetParameterTypes();
    
    #endregion
    
    #region ClassifierData

    /// <summary>
    /// Получение коллекции адресных объектов
    /// </summary>
    /// <returns>Коллекция адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    AddressObjectCollection GetAddressObjects(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о переподчинении адресных объектов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о переподчинении адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    AddressObjectDivisionCollection GetAddressObjectDivisions(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах адресных объектов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах адресных объектов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    AddressObjectParameterCollection GetAddressObjectParameters(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о иерарххии в административном делении
    /// </summary>
    /// <returns>Коллекция элементов с информацией о иерарххии в административном делении</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    AddressObjectAdmHierarchyCollection GetAddressObjectsAdmHierarchy(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о помещениях
    /// </summary>
    /// <returns>Коллекция элементов с информацией о помещениях</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ApartmentCollection GetApartments(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах помещений
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах помещений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ApartmentParameterCollection GetApartmentParameters(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о машино-местах
    /// </summary>
    /// <returns>Коллекция элементов с информацией о машино-местах</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    CarPlaceCollection GetCarPlaces(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах машино-места
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах машино-места</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    CarPlaceParameterCollection GetCarPlaceParameters(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией об истории изменений адресных элементов
    /// </summary>
    /// <returns>Коллекция элементов с информацией об истории изменений адресных элементов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ChangeHistoryCollection GetChangeHistory(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о строениях
    /// </summary>
    /// <returns>Коллекция элементов с информацией о строениях</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    HouseCollection GetHouses(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах строений
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах строений</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    HouseParameterCollection GetHouseParameters(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о иерарххии в муниципальном делении
    /// </summary>
    /// <returns>Коллекция элементов с информацией о иерарххии в муниципальном делении</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    MunHierarchyCollection GetMunHierarchy(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о нормативных документов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о нормативных документов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    NormativeDocumentCollection GetNormativeDocuments(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о реестре адресных элементов
    /// </summary>
    /// <returns>Коллекция элементов с информацией о реестре адресных элементов</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    ObjectRegistryCollection GetObjectsRegistry(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о комнатах
    /// </summary>
    /// <returns>Коллекция элементов с информацией о комнатах</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    RoomCollection GetRooms(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах комнат
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах комнат</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    RoomParameterCollection GetRoomParameters(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о земельных участках
    /// </summary>
    /// <returns>Коллекция элементов с информацией о земельных участках</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    SteadCollection GetSteads(Region region);

    /// <summary>
    /// Получение коллекции объектов с информацией о параметрах земельных участков
    /// </summary>
    /// <returns>Коллекция элементов с информацией о параметрах земельных участков</returns>
    /// <exception cref="FIASDataNotFoundException">Не удалось файл с данными</exception>
    SteadParameterCollection GetSteadParameters(Region region);

    #endregion
}