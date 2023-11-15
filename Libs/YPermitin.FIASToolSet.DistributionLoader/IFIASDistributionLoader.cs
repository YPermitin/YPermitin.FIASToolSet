using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.DistributionLoader.Exceptions;
using YPermitin.FIASToolSet.DistributionLoader.Models;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;

namespace YPermitin.FIASToolSet.DistributionLoader;

public interface IFIASDistributionLoader
{
    /// <summary>
    /// Информация о версии ФИАС, для которой выполняется операций установки / обновления
    /// </summary>
    FIASVersionInstallation VersionInstallation { get; }
    
    /// <summary>
    /// Проверка наличия активных процессов установки / обновления ФИАС
    /// </summary>
    /// <returns>Истина - есть активные процессы, Ложь в противном случае</returns>
    Task<bool> ActiveInstallationExists();

    /// <summary>
    /// Сброс статуса для зависших активных установок ФИАС.
    ///
    /// Обычно такая установка может быть только одна. Если она находится в статусе "Устанавливается"
    /// более 4 часов, то статус сбрасывается на "Новый".
    /// </summary>
    /// <returns>Список установок, для которых был сброшен статус зависший статус</returns>
    Task<List<FIASVersionInstallation>> FixStuckInstallationExists();
    
    /// <summary>
    /// Установка статуса текущей установки в "Новый"
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task SetInstallationToStatusNew();
    
    /// <summary>
    /// Определение версии ФИАС для установки или обновления
    /// </summary>
    /// <returns>Истина при успешном определении версии, Ложь в противном случае</returns>
    Task<bool> InitVersionInstallationToLoad();

    /// <summary>
    /// Скачивание и распаковка дистрибутива ФИАС
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task DownloadAndExtractDistribution(
        Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null);

    /// <summary>
    /// Удалениие архива данных для версии ФИАС
    /// </summary>
    void RemoveVersionDataArchive();
    
    /// <summary>
    /// Удаление каталога данных для версии ФИАС
    /// </summary>
    void RemoveVersionDataDirectory();
    
    /// <summary>
    /// Получает список кодов регионов, доступных для распаковки данных и загрузки
    /// </summary>
    /// <returns>Коллекция доступных регионов</returns>
    List<Region> GetAvailableRegions();

    /// <summary>
    /// Распаковка данных для указанного региона
    /// </summary>
    /// <param name="region">Информация о региоре</param>
    /// <returns>Путь к каталогу с данными по региону</returns>
    /// <exception cref="RegionNotFoundException">Регион с указанным кодом не найден</exception>
    string ExtractDataForRegion(Region region);

    /// <summary>
    /// Полусение пути к каталогу с данными региона
    /// </summary>
    /// <param name="region">Регион для распаковки данных</param>
    /// <returns>Путь к каталогу с данными по региону</returns>
    string GetDataDirectoryForRegion(Region region);

    /// <summary>
    /// Удаление каталога с данными классификатора по региону
    /// </summary>
    /// <param name="region">Регион для удаления каталога данных</param>
    void RemoveDistributionRegionDirectory(Region region);
    
    /// <summary>
    /// Установка статуса текущей установки в "Устанавливается"
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task SetInstallationToStatusInstalling();

    /// <summary>
    /// Установка статуса текущей установки в "Установлено"
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task SetInstallationToStatusInstalled();

    #region BaseCatalogs
    
    /// <summary>
    /// Загрузка типов адресных объектов
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadAddressObjectTypes();

    /// <summary>
    /// Загрузка типов помещений
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadApartmentTypes();

    /// <summary>
    /// Загрузка типов строений
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadHouseTypes();

    /// <summary>
    /// Загрузка видов нормативных документов
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadNormativeDocKinds();

    /// <summary>
    /// Загрузка типов нормативных документов
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadNormativeDocTypes();

    /// <summary>
    /// Загрузка уровней адресных объектов
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadObjectLevels();

    /// <summary>
    /// Загрузка типов операций
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadOperationTypes();

    /// <summary>
    /// Загрузка типов параметров
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadParameterTypes();

    /// <summary>
    /// Загрузка типов комнат
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task LoadRoomTypes();
    
    #endregion
    
    #region ClassifierData

    /// <summary>
    /// Загрузка адресных объектов по региону
    /// </summary>
    /// <param name="region">Регион для загрузки данных адресных объектов</param>
    Task LoadAddressObjects(Region region);

    /// <summary>
    /// Загрузка информации о переподчинении адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о переодчинении адресных объектов</param>
    Task LoadAddressObjectDivisions(Region region);

    /// <summary>
    /// Загрузка информации о параметрах адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах адресных объектов</param>
    Task LoadAddressObjectParameters(Region region);

    /// <summary>
    /// Загрузка информации о иерархии административного деления адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о иерархии административного деления адресных объектов</param>
    Task LoadAddressObjectsAdmHierarchy(Region region);

    /// <summary>
    /// Загрузка информации о иерархии муниципального деления адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о иерархии муниципального деления адресных объектов</param>
    Task LoadAddressObjectsMunHierarchy(Region region);

    /// <summary>
    /// Загрузка информации о квартирах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о квартирах</param>
    Task LoadApartments(Region region);

    /// <summary>
    /// Загрузка информации о параметрах квартир
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах квартир</param>
    Task LoadApartmentParameters(Region region);

    /// <summary>
    /// Загрузка информации о машино-местах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о машино-местах</param>
    Task LoadCarPlaces(Region region);
    
    /// <summary>
    /// Загрузка информации о параметрах машино-мест
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах машино-мест</param>
    Task LoadCarPlaceParameters(Region region);

    /// <summary>
    /// Загрузка информации о строениях
    /// </summary>
    /// <param name="region">Регион для загрузки данных о строениях</param>
    Task LoadHouses(Region region);
    
    /// <summary>
    /// Загрузка информации о параметрах строений
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах строений</param>
    Task LoadHouseParameters(Region region);

    /// <summary>
    /// Загрузка информации о комнатах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о комнатах</param>
    Task LoadRooms(Region region);
    
    /// <summary>
    /// Загрузка информации о параметрах комнат
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах комнат</param>
    Task LoadRoomParameters(Region region);

    /// <summary>
    /// Загрузка информации о земельных участках
    /// </summary>
    /// <param name="region">Регион для загрузки данных о земельных участках</param>
    Task LoadSteads(Region region);
    
    /// <summary>
    /// Загрузка информации о параметрах земельных участков
    /// </summary>
    /// <param name="region">Регион для загрузки данных о параметрах земельных участков</param>
    Task LoadSteadParameters(Region region);

    /// <summary>
    /// Загрузка информации о нормативных документах
    /// </summary>
    /// <param name="region">Регион для загрузки данных о нормативных документах</param>
    Task LoadNormativeDocuments(Region region);
    
    /// <summary>
    /// Загрузка истории изменений адресных объектов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о истории изменений адресных объектов</param>
    Task LoadChangeHistory(Region region);

    /// <summary>
    /// Загрузка реестра адресных элементов
    /// </summary>
    /// <param name="region">Регион для загрузки данных о реестре адресных элементов</param>
    Task LoadObjectsRegistry(Region region);

    #endregion
}