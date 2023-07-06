using System;
using System.Net.Http;
using System.Threading.Tasks;
using Downloader;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.DistributionBrowser.API
{
    internal class APIHelper : IAPIHelper
    {
        private static readonly string DefaultUserAgent = GeneralResources.DefaultUserAgent;
        private static readonly HttpClient APIClient;
        private static readonly DownloadConfiguration DownloaderConfiguration = new DownloadConfiguration()
        {
            ChunkCount = 8,
            ParallelDownload = true,
            RequestConfiguration = new RequestConfiguration()
            {
                UserAgent = DefaultUserAgent
            }
        };
        static APIHelper()
        {
            APIClient = new HttpClient();
            APIClient.DefaultRequestHeaders
                .UserAgent.TryParseAdd(DefaultUserAgent);
        }
        
        /// <summary>
        /// Загрузка файла по URL и сохранение его в файловой системе
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <param name="savePath">Путь для сохранения</param>
        /// <param name="onDownloadFileProgressChangedEvent">Событие с информацией о прогрессе загрузки файла</param>
        /// <returns>Асинхронная операция</returns>
        public async Task DownloadFileAsync(Uri uriFile, string savePath,
            Action<DownloadDistributionFileProgressChangedEventArgs> onDownloadFileProgressChangedEvent = null)
        {
            var downloader = new DownloadService(DownloaderConfiguration);
            downloader.DownloadStarted += (sender, args) =>
            {
                var eventArgs = new DownloadDistributionFileProgressChangedEventArgs(
                    state: DownloadDistributionFileProgressChangedEventType.Started,
                    progressPercentage: 0);
                onDownloadFileProgressChangedEvent?.Invoke(eventArgs);
            };
            downloader.DownloadProgressChanged += (sender, args) =>
            {
                var eventArgs = new DownloadDistributionFileProgressChangedEventArgs(
                    state: DownloadDistributionFileProgressChangedEventType.Downloading,
                    progressPercentage: args.ProgressPercentage);
                onDownloadFileProgressChangedEvent?.Invoke(eventArgs);
            };
            downloader.DownloadFileCompleted += (sender, args) =>
            {
                DownloadDistributionFileProgressChangedEventType state;
                Exception errorInfo = null;
                
                if (args.Cancelled)
                    state = DownloadDistributionFileProgressChangedEventType.Canceled;
                else if (args.Error != null)
                {
                    state = DownloadDistributionFileProgressChangedEventType.Failure;
                    errorInfo = args.Error;
                }
                else
                {
                    state = DownloadDistributionFileProgressChangedEventType.Compleated;
                }
                
                var eventArgs = new DownloadDistributionFileProgressChangedEventArgs(
                    state: state,
                    progressPercentage: 100,
                    errorInfo: errorInfo);
                onDownloadFileProgressChangedEvent?.Invoke(eventArgs);
            };
            await downloader.DownloadFileTaskAsync(uriFile.AbsoluteUri, savePath);
        }

        /// <summary>
        /// Получение содержимого по URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <returns>Строкове содержимое данных по URL</returns>
        public async Task<string> GetContentAsStringAsync(Uri uri)
        {
            var response = await APIClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        /// <summary>
        /// Проверка существования файла по URL
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <returns>Истина - файл сущестует и Ложь в противном случае</returns>
        public async Task<bool> FileByUrlExist(Uri uriFile)
        {
            string baseUrl = uriFile.AbsoluteUri.Replace(uriFile.AbsolutePath, string.Empty);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            try
            {
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uriFile.AbsolutePath));
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
