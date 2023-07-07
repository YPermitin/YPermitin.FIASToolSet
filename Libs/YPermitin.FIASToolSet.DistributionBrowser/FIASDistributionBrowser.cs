using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.DistributionBrowser.API;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.DistributionBrowser
{
    /// <summary>
    /// Объект работы с дистрибутивами ФИАС разных форматов и типов
    /// </summary>
    public sealed class FIASDistributionBrowser : IFIASDistributionBrowser
    {
        private const string APIBaseUrl = "http://fias.nalog.ru/WebServices/Public";
        
        private IAPIHelper _apiHelperObject;
        private IAPIHelper _apiHelper
        {
            get
            {
                return _apiHelperObject ??= new APIHelper(Options.MaximumDownloadSpeedBytesPerSecond);
            }
        }
        
        private readonly JsonSerializerOptions _serializerOptions;
        
        public FIASDistributionBrowserOptions Options { get; }
        
        public FIASDistributionBrowser()
        {
            Options ??= FIASDistributionBrowserOptions.Default;
            
            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new InternalDateTimeConverter("dd.MM.yyyy"));
        }
        
        public FIASDistributionBrowser(FIASDistributionBrowserOptions options) : this()
        {
            if (options != null)
            {
                Options = options;
            }
        }

        /// <summary>
        /// Информацию о последней версии файлов дистрибутива, доступных для скачивания
        /// </summary>
        /// <returns>Дистрибутив классификатора ФИАС</returns>
        public async Task<FIASDistributionInfo> GetLastDistributionInfo()
        {
            Uri methodUri = new Uri($"{APIBaseUrl}/GetLastDownloadFileInfo");
            string contentDownloadFileInfo = await _apiHelper.GetContentAsStringAsync(methodUri);
            DownloadFileInfo lastFileInfo = JsonSerializer.Deserialize<DownloadFileInfo>(contentDownloadFileInfo, _serializerOptions);
            
            return new FIASDistributionInfo(lastFileInfo, Options, _apiHelper);
        }
        
        /// <summary>
        /// Информацию обо всех версиях файлов дистрибутивов, доступных для скачивания
        /// </summary>
        /// <returns>Коллекция доступных дистрибутивов классификатора ФИАС</returns>
        public async Task<IReadOnlyList<FIASDistributionInfo>> GetAllDistributionInfo()
        {
            Uri methodUri = new Uri($"{APIBaseUrl}/GetAllDownloadFileInfo");
            string contentAllDownloadFileInfo = await _apiHelper.GetContentAsStringAsync(methodUri);
            List<DownloadFileInfo> allFileInfo = JsonSerializer.Deserialize<List<DownloadFileInfo>>(contentAllDownloadFileInfo, _serializerOptions);

            if(allFileInfo == null)
                return new List<FIASDistributionInfo>();
            
            return allFileInfo.Select(e => new FIASDistributionInfo(e, Options, _apiHelper)).ToList();
        }

        /// <summary>
        /// Очистка всех рабочих каталогов, связанных с браузером дистрибутивов
        /// </summary>
        public void ClearAllWorkingDirectories()
        {
            if (Directory.Exists(Options.WorkingDirectory))
            {
                Directory.Delete(Options.WorkingDirectory, true);
            }
        }
        
        private class InternalDateTimeConverter : JsonConverter<DateTime>
        {
            private readonly string _format;

            public InternalDateTimeConverter(string format)
            {
                _format = format;
            }

            public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
            {
                writer.WriteStringValue(date.ToString(_format));
            }

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (!DateTime.TryParseExact(reader.GetString() ?? string.Empty,
                        _format,
                        null,
                        DateTimeStyles.None,
                        out var output))
                {
                    output = DateTime.MinValue;
                }

                return output;
            }
        }
    }
}
