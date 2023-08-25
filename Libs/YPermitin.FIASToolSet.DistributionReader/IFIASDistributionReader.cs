using YPermitin.FIASToolSet.DistributionReader.DataCollections;
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

    #endregion
}