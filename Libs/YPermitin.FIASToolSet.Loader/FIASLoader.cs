using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.Loader.API;
using YPermitin.FIASToolSet.Loader.Models;

using System.Text.Json;

namespace YPermitin.FIASToolSet.Loader
{
    public class FIASLoader : IFIASLoader
    {
        #region Private Members

        private readonly IAPIHelper _apiHelper;

        #endregion

        #region Constructor

        public FIASLoader()
        {
            _apiHelper = new APIHelper();
        }

        #endregion

        #region Public Methods

        public async Task<DownloadFileInfo> GetLastDownloadFileInfo()
        {
            Uri methodUri = new Uri("http://fias.nalog.ru/WebServices/Public/GetLastDownloadFileInfo");
            string contentDownloadFileInfo = await _apiHelper.GetContentAsStringAsync(methodUri);
            DownloadFileInfo lastFileInfo = JsonSerializer.Deserialize<DownloadFileInfo>(contentDownloadFileInfo);

            return lastFileInfo;
        }

        public async Task<IReadOnlyList<DownloadFileInfo>> GetAllDownloadFileInfo()
        {
            Uri methodUri = new Uri("http://fias.nalog.ru/WebServices/Public/GetAllDownloadFileInfo");
            string contentAllDownloadFileInfo = await _apiHelper.GetContentAsStringAsync(methodUri);
            List<DownloadFileInfo> allFileInfo = JsonSerializer.Deserialize<List<DownloadFileInfo>>(contentAllDownloadFileInfo);

            return allFileInfo;
        }

        #endregion
    }
}
