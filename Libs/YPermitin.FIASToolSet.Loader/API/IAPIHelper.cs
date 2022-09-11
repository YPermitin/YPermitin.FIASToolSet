using System;
using System.Threading.Tasks;

namespace YPermitin.FIASToolSet.Loader.API
{
    internal interface IAPIHelper
    {
        Task DownloadFileAsync(Uri uriFile, string savePath);
        Task<string> GetContentAsStringAsync(Uri uri);
    }
}