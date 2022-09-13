using System;
using System.IO;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.Loader.API;

namespace YPermitin.FIASToolSet.Loader.Models
{
    /// <summary>
    /// Дистрибутив классификатора ФИАС
    /// </summary>
    public sealed class FIASDistributionInfo
    {
        private readonly IAPIHelper _apiHelper;

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
        /// Файлы дистрибутива КЛАДР 4 в формате 7Z
        /// </summary>
        public DistributionFileFormatInfo KLADR47z { get; }

        internal FIASDistributionInfo(DownloadFileInfo downloadFileInfo)
        {
            _apiHelper = new APIHelper();
            
            VersionId = downloadFileInfo.VersionId;
            TextVersion = downloadFileInfo.TextVersion;
            Date = downloadFileInfo.Date;

            FIASDbf = new DistributionFileFormatInfo(
                _apiHelper,
                downloadFileInfo.FiasCompleteDbfUrl,
                downloadFileInfo.FiasDeltaDbfUrl);

            FIASXml = new DistributionFileFormatInfo(
                _apiHelper,
                downloadFileInfo.FiasCompleteXmlUrl,
                downloadFileInfo.FiasDeltaXmlUrl);

            GARFIASXml = new DistributionFileFormatInfo(
                _apiHelper,
                downloadFileInfo.GarXMLFullURL,
                downloadFileInfo.GarXMLDeltaURL);

            KLADR4Arj = new DistributionFileFormatInfo(
                _apiHelper,
                downloadFileInfo.Kladr4ArjUrl,
                null);

            KLADR47z = new DistributionFileFormatInfo(
                _apiHelper,
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
            /// <returns>Асинхронная операция</returns>
            public async Task SaveCompleteFileAsync(string fileToSave)
            {
                FileInfo fileToSaveInfo = new FileInfo(fileToSave);
                if (fileToSaveInfo.Directory != null)
                {
                    if (!fileToSaveInfo.Directory.Exists)
                        fileToSaveInfo.Directory.Create();
                }

                await _apiHelper.DownloadFileAsync(Complete, fileToSave);
            }

            /// <summary>
            /// Проверка существования файла с информацией о последней версии файлов дистрибутива, доступных для скачивания
            /// </summary>
            /// <returns>Истина - существует и Ложь в противном случае</returns>
            public async Task<bool> LastDistributionInfoExists()
            {
                if (Complete != null)
                    return false;

                return _apiHelper.FileByUrlExist(Complete);
            }

            /// <summary>
            /// Сохранение дистрибутива с изменениями в указанный файл
            /// </summary>
            /// <param name="fileToSave">Путь к файлу для сохранения</param>
            /// <returns>Асинхронная операция</returns>
            public async Task SaveDeltaFileAsync(string fileToSave)
            {
                FileInfo fileToSaveInfo = new FileInfo(fileToSave);
                if (fileToSaveInfo.Directory != null)
                {
                    if (!fileToSaveInfo.Directory.Exists)
                        fileToSaveInfo.Directory.Create();
                }

                await _apiHelper.DownloadFileAsync(Delta, fileToSave);
            }

            /// <summary>
            /// Проверка существования файла с изменениями
            /// </summary>
            /// <returns>Истина - существует и Ложь в противном случае</returns>
            public async Task<bool> DeltaDistributionInfoExists()
            {
                if (Delta != null)
                    return false;

                return _apiHelper.FileByUrlExist(Delta);
            }
        }
    }
}
