using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Downloader;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.DistributionBrowser.API
{
    internal class APIHelper : IAPIHelper
    {
        private static readonly HttpClient APIClient;
        private const int DownloadFileActivityWaitMs = 600000;
        private static readonly string DefaultUserAgent = GeneralResources.DefaultUserAgent;
        private static readonly DownloadConfiguration DefaultDownloaderConfiguration;
        private static DownloadConfiguration GenerateDownloaderConfiguration(
            long maximumDownloadSpeedBytesPerSecond = long.MinValue)
        {
            return new DownloadConfiguration()
            {
                MaximumBytesPerSecond = maximumDownloadSpeedBytesPerSecond,
                ChunkCount = 8,
                ParallelDownload = true,
                RequestConfiguration = new RequestConfiguration()
                {
                    UserAgent = DefaultUserAgent
                }
            };
        }

        private readonly DownloadConfiguration _downloaderConfiguration;
        
        static APIHelper()
        {
            APIClient = new HttpClient();
            APIClient.DefaultRequestHeaders
                .UserAgent.TryParseAdd(DefaultUserAgent);
            DefaultDownloaderConfiguration = GenerateDownloaderConfiguration();
        }

        public APIHelper()
        {
            _downloaderConfiguration = DefaultDownloaderConfiguration;
        }

        public APIHelper(long maximumDownloadSpeedBytesPerSecond) : this()
        {
            if (_downloaderConfiguration.MaximumBytesPerSecond != maximumDownloadSpeedBytesPerSecond
                && maximumDownloadSpeedBytesPerSecond != 0
                && maximumDownloadSpeedBytesPerSecond != long.MaxValue)
            {
                _downloaderConfiguration = GenerateDownloaderConfiguration(maximumDownloadSpeedBytesPerSecond);
            }
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
            using (AutoResetEvent downloadWaitHandle = new AutoResetEvent(false))
            {
                Exception downloaderException = null;
                var downloader = new DownloadService(_downloaderConfiguration);

                downloader.DownloadStarted += (sender, args) =>
                {
                    downloadWaitHandle.Set();
                    
                    var eventArgs = new DownloadDistributionFileProgressChangedEventArgs(
                        state: DownloadDistributionFileProgressChangedEventType.Started,
                        progressPercentage: 0);
                    onDownloadFileProgressChangedEvent?.Invoke(eventArgs);
                };
                downloader.DownloadProgressChanged += (sender, args) =>
                {
                    downloadWaitHandle.Set();
                    
                    var eventArgs = new DownloadDistributionFileProgressChangedEventArgs(
                        state: DownloadDistributionFileProgressChangedEventType.Downloading,
                        progressPercentage: args.ProgressPercentage);
                    onDownloadFileProgressChangedEvent?.Invoke(eventArgs);
                };
                downloader.DownloadFileCompleted += (sender, args) =>
                {
                    downloadWaitHandle.Set();
                    
                    DownloadDistributionFileProgressChangedEventType state;
                    Exception errorInfo = null;

                    if (args.Cancelled)
                        state = DownloadDistributionFileProgressChangedEventType.Canceled;
                    else if (args.Error != null)
                    {
                        state = DownloadDistributionFileProgressChangedEventType.Failure;
                        errorInfo = args.Error;
                        downloaderException = args.Error;
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

                CancellationTokenSource downloadFileCancellationTokenSource = new CancellationTokenSource();
                var downloadFileTask = downloader.DownloadFileTaskAsync(
                    address: uriFile.AbsoluteUri,
                    fileName: savePath,
                    cancellationToken: downloadFileCancellationTokenSource.Token);

                do
                {
                    if (!downloadWaitHandle.WaitOne(DownloadFileActivityWaitMs))
                    {
                        downloadFileCancellationTokenSource.Cancel();
                        downloadFileCancellationTokenSource.Token.WaitHandle.WaitOne();
                    }

                    await Task.Delay(1000, downloadFileCancellationTokenSource.Token);
                } while (!downloadFileTask.IsCompleted);

                if (downloadFileTask.IsCompleted)
                {
                    if (downloadFileTask.IsFaulted)
                    {
                        throw downloadFileTask.Exception;
                    }
                    if (downloadFileTask.IsCanceled)
                    {
                        throw new OperationCanceledException("Загрузка файла отменена.");
                    }
                    if (downloader.Status == DownloadStatus.Failed)
                    {
                        if (downloaderException != null)
                        {
                            throw downloaderException;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получение содержимого по URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="attempts">Количество попыток отправки запросов при ошибках связи</param>
        /// <returns>Строкове содержимое данных по URL</returns>
        public async Task<string> GetContentAsStringAsync(Uri uri, int attempts = 3)
        {
            int currentAttempt = 1;
            string content = null;

            while (currentAttempt <= attempts)
            {              
                try
                {
                    var response = await APIClient.GetAsync(uri);
                    response.EnsureSuccessStatusCode();

                    content = await response.Content.ReadAsStringAsync();
                    break;
                }
                catch (Exception ex)
                {
                    // Ошибки соединения и транспорта обраббатываются с учетом попыток отправки запроса.
                    if(ex is HttpRequestException && ex.InnerException is IOException)
                    {
                        if (currentAttempt >= attempts)
                        {
                            throw;
                        }

                        await Task.Delay(1000);
                        currentAttempt++;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if(content == null)
            {
                throw new Exception("Content does not exists.");
            }
            
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
