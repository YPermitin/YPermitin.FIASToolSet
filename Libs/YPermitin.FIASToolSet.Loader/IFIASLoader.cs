using System.Collections.Generic;
using System.Threading.Tasks;
using YPermitin.FIASToolSet.Loader.Models;

namespace YPermitin.FIASToolSet.Loader
{
    public interface IFIASLoader
    {
        Task<DownloadFileInfo> GetLastDownloadFileInfo();
        Task<IReadOnlyList<DownloadFileInfo>> GetAllDownloadFileInfo();
    }
}
