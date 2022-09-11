using System;
using System.IO;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.Loader.API;

namespace YPermitin.FIASToolSet.Loader.Models
{
    public sealed class DownloadFileInfo
    {
        #region Private Members

        private readonly IAPIHelper _apiHelper;

        #endregion

        #region Public Members

        public int VersionId { get; set; }
        public string TextVersion { get; set; }

        public DateTime VersionDate
        {
            get
            {
                DateTime versionDate = DateTime.MinValue;

                if (TextVersion != null)
                {
                    string sourceVersionDate = TextVersion.Substring(TextVersion.Length - 10, 10);
                    DateTime.TryParse(sourceVersionDate, out versionDate);
                }
                
                return versionDate;
            }
        }
        public Uri FiasCompleteDbfUrl { get; set; }
        public Uri FiasCompleteXmlUrl { get; set; }
        public Uri FiasDeltaDbfUrl { get; set; }
        public Uri FiasDeltaXmlUrl { get; set; }
        public Uri Kladr4ArjUrl { get; set; }
        public Uri Kladr47ZUrl { get; set; }

        #endregion

        #region Constructor

        public DownloadFileInfo()
        {
            _apiHelper = new APIHelper();
        }

        #endregion

        #region Public Methods

        public async Task SaveFiasCompleteDbfFileAsync(string fileToSave)
        {
            FileInfo fileToSaveInfo = new FileInfo(fileToSave);
            if (fileToSaveInfo.Directory != null)
            {
                if (!fileToSaveInfo.Directory.Exists)
                    fileToSaveInfo.Directory.Create();
            }

            await _apiHelper.DownloadFileAsync(FiasCompleteDbfUrl, fileToSave);
        }
        public async Task SaveFiasCompleteDbfToDirectoryAsync(string directoryToSave)
        {
            string fileToSave = Path.Combine(directoryToSave, "FiasCompleteDbf.dbf");
            await SaveFiasCompleteDbfFileAsync(fileToSave);
        }

        public async Task SaveFiasCompleteXmlFileAsync(string fileToSave)
        {
            FileInfo fileToSaveInfo = new FileInfo(fileToSave);
            if (fileToSaveInfo.Directory != null)
            {
                if (!fileToSaveInfo.Directory.Exists)
                    fileToSaveInfo.Directory.Create();
            }

            await _apiHelper.DownloadFileAsync(FiasCompleteXmlUrl, fileToSave);
        }
        public async Task SaveFiasCompleteXmlToDirectoryAsync(string directoryToSave)
        {
            string fileToSave = Path.Combine(directoryToSave, "SaveFiasCompleteXml.xml");
            await SaveFiasCompleteXmlFileAsync(fileToSave);
        }

        public async Task SaveFiasDeltaDbFileAsync(string fileToSave)
        {
            FileInfo fileToSaveInfo = new FileInfo(fileToSave);
            if (fileToSaveInfo.Directory != null)
            {
                if (!fileToSaveInfo.Directory.Exists)
                    fileToSaveInfo.Directory.Create();
            }

            await _apiHelper.DownloadFileAsync(FiasDeltaDbfUrl, fileToSave);
        }
        public async Task SaveFiasDeltaDbToDirectoryAsync(string directoryToSave)
        {
            string fileToSave = Path.Combine(directoryToSave, "SaveFiasDeltaDb.dbf");
            await SaveFiasDeltaDbFileAsync(fileToSave);
        }

        public async Task SaveFiasDeltaXmlFileAsync(string fileToSave)
        {
            FileInfo fileToSaveInfo = new FileInfo(fileToSave);
            if (fileToSaveInfo.Directory != null)
            {
                if (!fileToSaveInfo.Directory.Exists)
                    fileToSaveInfo.Directory.Create();
            }

            await _apiHelper.DownloadFileAsync(FiasDeltaXmlUrl, fileToSave);
        }
        public async Task SaveFiasDeltaXmlToDirectoryAsync(string directoryToSave)
        {
            string fileToSave = Path.Combine(directoryToSave, "FiasDeltaXml.xml");
            await SaveFiasDeltaXmlFileAsync(fileToSave);
        }

        public async Task SaveKladr4ArjFileAsync(string fileToSave)
        {
            FileInfo fileToSaveInfo = new FileInfo(fileToSave);
            if (fileToSaveInfo.Directory != null)
            {
                if (!fileToSaveInfo.Directory.Exists)
                    fileToSaveInfo.Directory.Create();
            }

            await _apiHelper.DownloadFileAsync(Kladr4ArjUrl, fileToSave);
        }
        public async Task SaveKladr4ArjToDirectoryAsync(string directoryToSave)
        {
            string fileToSave = Path.Combine(directoryToSave, "Kladr4Arj.arj");
            await SaveKladr4ArjFileAsync(fileToSave);
        }

        public async Task SaveKladr47ZFileAsync(string fileToSave)
        {
            FileInfo fileToSaveInfo = new FileInfo(fileToSave);
            if (fileToSaveInfo.Directory != null)
            {
                if (!fileToSaveInfo.Directory.Exists)
                    fileToSaveInfo.Directory.Create();
            }

            await _apiHelper.DownloadFileAsync(Kladr47ZUrl, fileToSave);
        }
        public async Task SaveKladr47ZToDirectoryAsync(string directoryToSave)
        {
            string fileToSave = Path.Combine(directoryToSave, "Kladr47Z.7z");
            await SaveKladr47ZFileAsync(fileToSave);
        }

        #endregion
    }
}
