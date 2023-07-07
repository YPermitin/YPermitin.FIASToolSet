using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.DistributionBrowser.API;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionBrowser.Helpers;

namespace YPermitin.FIASToolSet.DistributionBrowser.Models
{
    /// <summary>
    /// Дистрибутив классификатора ФИАС
    /// </summary>
    public sealed class FIASDistributionInfo
    {
        private readonly IAPIHelper _apiHelper;
        private readonly FIASDistributionBrowserOptions _options;
        
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
        /// Получение пути к скачанному файлу дистрибутива по типу файла
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <returns>Путь к скачанному файлу дистрибутива</returns>
        public string GetLocalPathByFileType(DistributionFileType fileType)
        {
            string pathToVersionDirectory = GetPathToVersionDirectory();
            
            string localPathByFileType = Path.Combine(
                pathToVersionDirectory,
                $"{fileType.ToString()}.zip"
            );

            return localPathByFileType;
        }

        /// <summary>
        /// Удалить рабочий каталог версии дистрибутива
        ///
        /// Удаляет рабочий каталог версии дистрибутива со всеми вложенными файлами.
        /// </summary>
        public void RemoveVersionWorkingDirectory()
        {
            string pathToVersionDirectory = GetPathToVersionDirectory();
            
            DirectoryInfo versionWorkingDirectory = new DirectoryInfo(pathToVersionDirectory);
            versionWorkingDirectory.Delete(true);
        }
        
        /// <summary>
        /// Скачивание файла дистрибутивая по типу
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <param name="onDownloadFileProgressChangedEvent">Событие с информацией о прогрессе загрузки файла</param>
        public async Task DownloadDistributionByFileTypeAsync(
            DistributionFileType fileType,
            Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null)
        {
            string pathToSaveFile = GetLocalPathByFileType(fileType);
            FileInfo fileToSave = new FileInfo(pathToSaveFile);

            if (fileToSave.Exists)
            {
                if (fileToSave.Extension.ToUpperInvariant() == ".ZIP")
                {
                    if (ZipHelper.IsZipValid(fileToSave.FullName))
                    {
                        onDownloadFileProgressChangedEvent?.Invoke(new DownloadDistributionFileProgressChangedEventArgs(
                            DownloadDistributionFileProgressChangedEventType.AlreadyExists,
                            1001));
                        return;
                    }
                }
                else
                {
                    onDownloadFileProgressChangedEvent?.Invoke(new DownloadDistributionFileProgressChangedEventArgs(
                        DownloadDistributionFileProgressChangedEventType.AlreadyExists,
                        1001));
                    return;
                }
            }
            
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

        /// <summary>
        /// Получение списка регионов, которые содержатся в архиве дистрибутива
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <returns>Массив кодов регионов, для которых есть даные в файле дистрибутива</returns>
        public string[] GetAvailableRegions(DistributionFileType fileType)
        {
            List<string> regions = new List<string>();
            
            string pathToFile = GetLocalPathByFileType(fileType);
            if (File.Exists(pathToFile))
            {
                using (var zipFile = ZipFile.OpenRead(pathToFile))
                {
                    var entries = zipFile.Entries;
                    foreach (var entry in entries)
                    {
                        int indexEndRegionName = entry.FullName.IndexOf("/");
                        if (indexEndRegionName <= 0) continue;

                        string regionDirectoryName = entry.FullName.Substring(0, indexEndRegionName);
                        if (!regions.Contains(regionDirectoryName))
                            regions.Add(regionDirectoryName);
                    }
                }
            }

            return regions.OrderBy(e => e).ToArray();
        }

        /// <summary>
        /// Получение пути к каталогу, куда распаковывается архив дистрибутива
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <returns>Путь к каталогу для распаковки архива дистрибутива</returns>
        public string GetExtractedDirectory(DistributionFileType fileType)
        {
            string pathToFile = GetLocalPathByFileType(fileType);
            FileInfo fileInfo = new FileInfo(pathToFile);
            
            string extractedDirectory = Path.Combine(
                fileInfo.Directory.FullName,
                fileInfo.Name.Replace(fileInfo.Extension, String.Empty)
            );

            return extractedDirectory;
        }

        /// <summary>
        /// Распаковка архива с файлами дистрибутива
        /// </summary>
        /// <param name="fileType">Тип файла дистрибутива</param>
        /// <param name="onlyBaseFiles">Распаковать только базовые файлы (файлы по регионам не будут распакованы)</param>
        /// <param name="regions">Массив кодов регионов для распаковки</param>
        public void ExtractDistributionFile(
            DistributionFileType fileType,
            bool onlyBaseFiles = false,
            string[] regions = null)
        {
            string pathToFile = GetLocalPathByFileType(fileType);
            FileInfo fileInfo = new FileInfo(pathToFile);

            if (fileInfo.Exists)
            {
                string directoryToExtract = GetExtractedDirectory(fileType);
                if (Directory.Exists(directoryToExtract))
                {
                    Directory.Delete(directoryToExtract, true);
                }
                Directory.CreateDirectory(directoryToExtract);
                
                using (var zipFile = ZipFile.OpenRead(pathToFile))
                {
                    var entries = zipFile.Entries;
                    foreach (var entry in entries)
                    {
                        int indexEndRegionName = entry.FullName.IndexOf("/");
                        if (indexEndRegionName <= 0)
                        {
                            string fileNameToExtract = Path.Combine(
                                directoryToExtract,
                                entry.FullName
                            );
                            entry.ExtractToFile(fileNameToExtract);
                        }
                        else
                        {
                            bool doExtract = true;

                            if (onlyBaseFiles)
                            {
                                doExtract = false;
                            }
                            else if (regions != null)
                            {
                                string regionDirectoryName = entry.FullName.Substring(0, indexEndRegionName);
                                if (int.TryParse(regionDirectoryName, out _))
                                {
                                    doExtract = regions.Contains(regionDirectoryName);   
                                }
                            }

                            if (doExtract)
                            {
                                string fileNameToExtract = Path.Combine(
                                    directoryToExtract,
                                    entry.FullName
                                );
                                FileInfo fileInfoToExtract = new FileInfo(fileNameToExtract);
                                if (fileInfoToExtract.Directory != null)
                                {
                                    if(!fileInfoToExtract.Directory.Exists)
                                        fileInfoToExtract.Directory.Create();
                                }
                                entry.ExtractToFile(fileNameToExtract);
                            }
                        }
                    }
                }
            }
        }

        internal FIASDistributionInfo(DownloadFileInfo downloadFileInfo, FIASDistributionBrowserOptions options, IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _options = options;
            
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

        private string GetPathToVersionDirectory()
        {
            string pathToVersionDirectory = Path.Combine(
                _options.WorkingDirectory,
                VersionId.ToString()
            );
            if (!Directory.Exists(pathToVersionDirectory))
            {
                Directory.CreateDirectory(pathToVersionDirectory);
            }

            return pathToVersionDirectory;
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
