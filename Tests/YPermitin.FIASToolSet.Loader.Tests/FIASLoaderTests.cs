using YPermitin.FIASToolSet.Loader.Models;

namespace YPermitin.FIASToolSet.Loader.Tests
{
    public class FIASLoaderTests
    {
        [Fact]
        public async Task GetLastDownloadFileInfoTest()
        {
            IFIASLoader loader = new FIASLoader();
            DownloadFileInfo lastInfo = await loader.GetLastDownloadFileInfo();

            Assert.NotNull(lastInfo);
            Assert.True(lastInfo.VersionId > 0);
            Assert.True(lastInfo.VersionDate > DateTime.MinValue);
        }

        [Fact]
        public async Task GetAllDownloadFileInfoTest()
        {
            IFIASLoader loader = new FIASLoader();
            IReadOnlyList<DownloadFileInfo> allInfo = await loader.GetAllDownloadFileInfo();

            Assert.NotNull(allInfo);
            Assert.True(allInfo.Count > 0);
        }
    }
}
