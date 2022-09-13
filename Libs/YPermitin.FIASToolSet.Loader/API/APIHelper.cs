using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace YPermitin.FIASToolSet.Loader.API
{
    internal class APIHelper : IAPIHelper
    {
        private readonly HttpClient _apiClient;


        public APIHelper()
        {
            _apiClient = new HttpClient();
        }

        /// <summary>
        /// Загрузка файла по URL и сохранение его в файловой системе
        /// </summary>
        /// <param name="uriFile">URL файла</param>
        /// <param name="savePath">Путь для сохранения</param>
        /// <returns>Асинхронная операция</returns>
        public async Task DownloadFileAsync(Uri uriFile, string savePath)
        {
            using (HttpResponseMessage response = _apiClient.GetAsync(uriFile, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                response.EnsureSuccessStatusCode();

                await using (Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var buffer = new byte[8192];
                    var isMoreToRead = true;

                    do
                    {
                        var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            await fileStream.WriteAsync(buffer, 0, read);
                        }
                    }
                    while (isMoreToRead);
                }
            }
        }

        /// <summary>
        /// Получение содержимого по URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <returns>Строкове содержимое данных по URL</returns>
        public async Task<string> GetContentAsStringAsync(Uri uri)
        {
            var response = await _apiClient.GetAsync(uri);
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
