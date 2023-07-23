using YPermitin.FIASToolSet.DistributionBrowser.Models;
using YPermitin.FIASToolSet.Storage.Core.Models;

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
    /// Установка статуса текущей установки в "Устанавливается"
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task SetInstallationToStatusInstalling();

    /// <summary>
    /// Установка статуса текущей установки в "Установлено"
    /// </summary>
    /// <returns>Объект асинхронной операции</returns>
    Task SetInstallationToStatusInstalled();

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
}