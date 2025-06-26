using System;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.DistributionBrowser.API
{
    internal interface IAPIHelper
    {
        /// <summary>
        /// Загрузка файла по URL и сохранение его в файловой системе
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <param name="savePath">Путь для сохранения</param>
        /// <param name="onDownloadFileProgressChangedEvent">Событие с информацией о прогрессе загрузки файла</param>
        /// <returns>Асинхронная операция</returns>
        Task DownloadFileAsync(Uri uriFile, string savePath,
            Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null);

        /// <summary>
        /// Получение содержимого по URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="attempts">Количество попыток отправки запросов при ошибках связи</param>
        /// <returns>Строкове содержимое данных по URL</returns>
        Task<string> GetContentAsStringAsync(Uri uri, int attempts = 3);

        /// <summary>
        /// Проверка существования файла по URL
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <returns>Истина - файл сущестует и Ложь в противном случае</returns>
        Task<bool> FileByUrlExist(Uri uriFile);
    }
}