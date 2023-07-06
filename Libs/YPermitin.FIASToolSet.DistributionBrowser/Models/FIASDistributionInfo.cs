using System;
using System.IO;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.DistributionBrowser.API;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;

namespace YPermitin.FIASToolSet.DistributionBrowser.Models
{
    /// <summary>
    /// Дистрибутив классификатора ФИАС
    /// </summary>
    public sealed class FIASDistributionInfo
    {
        /// <summary>
        /// Идентификатор версии (в прямых выгрузках дата выгрузки вида yyyyMMdd)
        /// </summary>
        public int VersionId { get; }

        /// <summary>
        /// Описание версии файла в текстовом виде
        /// </summary>
        public string TextVersion { get; }

        /// <summary>
        /// Дата выгрузки (dd.MM.yyyy)
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Файлы дистрибутива ФИАС в формате DBF
        /// </summary>
        public DistributionFileFormatInfo FIASDbf { get; }

        /// <summary>
        /// Файлы дистрибутива ФИАС в формате XML
        /// </summary>
        public DistributionFileFormatInfo FIASXml { get; }

        /// <summary>
        /// Файлы дистрибутива ГАР ФИАС в формате XML
        /// </summary>
        public DistributionFileFormatInfo GARFIASXml { get; }

        /// <summary>
        /// Файлы дистрибутива КЛАДР 4 в формате ARJ
        /// </summary>
        public DistributionFileFormatInfo KLADR4Arj { get; }

        /// <summary>
        /// Получение адреса файла для скачивания по типу
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <returns>Адрес для скачивания файла дистрибутива</returns>
        public Uri GetUrlByFileType(DistributionFileType fileType)
        {
            Uri fileUrl = null;
            
            switch (fileType)
            {
                case DistributionFileType.FIASDbfComplete:
                    fileUrl = FIASDbf.Complete;
                    break;
                case DistributionFileType.FIASDbfDelta:
                    fileUrl = FIASDbf.Delta;
                    break;
                case DistributionFileType.FIASXmlComplete:
                    fileUrl = FIASXml.Complete;
                    break;
                case DistributionFileType.FIASXmlDelta:
                    fileUrl = FIASXml.Delta;
                    break;
                case DistributionFileType.GARFIASXmlComplete:
                    fileUrl = GARFIASXml.Complete;
                    break;
                case DistributionFileType.GARFIASXmlDelta:
                    fileUrl = GARFIASXml.Delta;
                    break;
                case DistributionFileType.KLADR47zComplete:
                    fileUrl = KLADR47z.Complete;
                    break;
                case DistributionFileType.KLADR4ArjComplete:
                    fileUrl = KLADR4Arj.Complete;
                    break;
            }

            return fileUrl;
        }

        /// <summary>
        /// Скачивание файла дистрибутивая по типу
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <param name="pathToSaveFile">Путь для сохранрения файла</param>
        /// <param name="onDownloadFileProgressChangedEvent">Событие с информацией о прогрессе загрузки файла</param>
        public async Task DownloadDistributionByFileTypeAsync(
            DistributionFileType fileType, 
            string pathToSaveFile,
            Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null)
        {
            switch (fileType)
            {
                case DistributionFileType.FIASDbfComplete:
                    await FIASDbf.SaveCompleteFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.FIASDbfDelta:
                    await FIASDbf.SaveDeltaFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.FIASXmlComplete:
                    await FIASXml.SaveCompleteFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.FIASXmlDelta:
                    await FIASXml.SaveDeltaFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.KLADR4ArjComplete:
                    await KLADR4Arj.SaveCompleteFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.KLADR47zComplete:
                    await KLADR47z.SaveDeltaFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.GARFIASXmlComplete:
                    await GARFIASXml.SaveCompleteFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
                case DistributionFileType.GARFIASXmlDelta:
                    await GARFIASXml.SaveDeltaFileAsync(pathToSaveFile, onDownloadFileProgressChangedEvent);
                    break;
            }
        }

        /// <summary>
        /// Файлы дистрибутива КЛАДР 4 в формате 7Z
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public DistributionFileFormatInfo KLADR47z { get; }

        internal FIASDistributionInfo(DownloadFileInfo downloadFileInfo)
        {
            IAPIHelper apiHelper = new APIHelper();
            
            VersionId = downloadFileInfo.VersionId;
            TextVersion = downloadFileInfo.TextVersion;
            Date = downloadFileInfo.Date;

            FIASDbf = new DistributionFileFormatInfo(
                apiHelper,
                downloadFileInfo.FiasCompleteDbfUrl,
                downloadFileInfo.FiasDeltaDbfUrl);

            FIASXml = new DistributionFileFormatInfo(
                apiHelper,
                downloadFileInfo.FiasCompleteXmlUrl,
                downloadFileInfo.FiasDeltaXmlUrl);

            GARFIASXml = new DistributionFileFormatInfo(
                apiHelper,
                downloadFileInfo.GarXMLFullURL,
                downloadFileInfo.GarXMLDeltaURL);

            KLADR4Arj = new DistributionFileFormatInfo(
                apiHelper,
                downloadFileInfo.Kladr4ArjUrl,
                null);

            KLADR47z = new DistributionFileFormatInfo(
                apiHelper,
                downloadFileInfo.Kladr47ZUrl,
                null);
        }

        /// <summary>
        /// Файлы дистрибутива ФИАС
        /// </summary>
        public class DistributionFileFormatInfo
        {
            private readonly IAPIHelper _apiHelper;

            /// <summary>
            /// URL полной версии дистрибутива
            /// </summary>
            public Uri Complete { get; }

            /// <summary>
            /// URL дельта версии дистрибутива
            /// </summary>
            public Uri Delta { get; }

            internal DistributionFileFormatInfo(IAPIHelper apiHelper, Uri complete, Uri delta)
            {
                _apiHelper = apiHelper;
                Complete = complete;
                Delta = delta;
            }

            /// <summary>
            /// Сохранение полного дистрибутива в указанный файл
            /// </summary>
            /// <param name="fileToSave">Путь к файлу для сохранения</param>
            /// <param name="onDownloadFileProgressChangedEvent">Событие с информацией о прогрессе загрузки файла</param>
            /// <returns>Асинхронная операция</returns>
            public async Task SaveCompleteFileAsync(string fileToSave,
                Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null)
            {
                FileInfo fileToSaveInfo = new FileInfo(fileToSave);
                if (fileToSaveInfo.Directory != null)
                {
                    if (!fileToSaveInfo.Directory.Exists)
                        fileToSaveInfo.Directory.Create();
                }
                
                await _apiHelper.DownloadFileAsync(Complete, fileToSave, onDownloadFileProgressChangedEvent);
            }

            /// <summary>
            /// Проверка существования файла с информацией о последней версии файлов дистрибутива, доступных для скачивания
            /// </summary>
            /// <returns>Истина - существует и Ложь в противном случае</returns>
            public async Task<bool> LastDistributionInfoExists()
            {
                if (Complete != null)
                    return false;

                return await _apiHelper.FileByUrlExist(Complete);
            }

            /// <summary>
            /// Сохранение дистрибутива с изменениями в указанный файл
            /// </summary>
            /// <param name="fileToSave">Путь к файлу для сохранения</param>
            /// <param name="onDownloadFileProgressChangedEvent">Событие с информацией о прогрессе загрузки файла</param>
            /// <returns>Асинхронная операция</returns>
            public async Task SaveDeltaFileAsync(string fileToSave,
                Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null)
            {
                FileInfo fileToSaveInfo = new FileInfo(fileToSave);
                if (fileToSaveInfo.Directory != null)
                {
                    if (!fileToSaveInfo.Directory.Exists)
                        fileToSaveInfo.Directory.Create();
                }

                await _apiHelper.DownloadFileAsync(Delta, fileToSave, onDownloadFileProgressChangedEvent);
            }

            /// <summary>
            /// Проверка существования файла с изменениями
            /// </summary>
            /// <returns>Истина - существует и Ложь в противном случае</returns>
            public async Task<bool> DeltaDistributionInfoExists()
            {
                if (Delta != null)
                    return false;

                return await _apiHelper.FileByUrlExist(Delta);
            }
        }
    }
}
