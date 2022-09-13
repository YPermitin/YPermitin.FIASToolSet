using System;
using System.Threading.Tasks;

namespace YPermitin.FIASToolSet.Loader.API
{
    internal interface IAPIHelper
    {
        /// <summary>
        /// Загрузка файла по URL и сохранение его в файловой системе
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <param name="savePath">Путь для сохранения</param>
        /// <returns>Асинхронная операция</returns>
        Task DownloadFileAsync(Uri uriFile, string savePath);

        /// <summary>
        /// Получение содержимого по URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <returns>Строкове содержимое данных по URL</returns>
        Task<string> GetContentAsStringAsync(Uri uri);

        /// <summary>
        /// Проверка существования файла по URL
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <returns>Истина - файл сущестует и Ложь в противном случае</returns>
        Task<bool> FileByUrlExist(Uri uriFile);
    }
}