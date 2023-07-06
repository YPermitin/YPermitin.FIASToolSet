using YPermitin.FIASToolSet.DistributionBrowser.Enums;

namespace YPermitin.FIASToolSet.DistributionBrowser.Tests
{
    public class FIASDistributionBrowserTests
    {
        [Fact]
        public async Task GetLastDistributionInfoTest()
        {
            IFIASDistributionBrowser loader = new FIASDistributionBrowser();
            var lastInfo = await loader.GetLastDistributionInfo();

            Assert.NotNull(lastInfo);
            Assert.True(lastInfo.VersionId > 0);
            Assert.NotNull(lastInfo.TextVersion);
            Assert.True(lastInfo.Date > DateTime.MinValue);
            Assert.NotNull(lastInfo.FIASDbf);
            Assert.NotNull(lastInfo.FIASXml);
            Assert.NotNull(lastInfo.GARFIASXml);
            Assert.NotNull(lastInfo.KLADR47z);
            Assert.NotNull(lastInfo.KLADR4Arj);
        }

        [Fact]
        public async Task GetAllDistributionInfoTest()
        {
            IFIASDistributionBrowser loader = new FIASDistributionBrowser();
            var allInfo = await loader.GetAllDistributionInfo();

            Assert.NotNull(allInfo);
            Assert.True(allInfo.Count > 0);
        }

        [Fact]
        public async Task DownloadLastGarXmlDeltaDistribution()
        {
            IFIASDistributionBrowser loader = new FIASDistributionBrowser();
            var lastInfo = await loader.GetLastDistributionInfo();

            var tempFileToDownload = Path.Combine(
                Path.GetTempPath(),
                "FIAS_FULL.zip");

            if (lastInfo != null)
            {
                await lastInfo.DownloadDistributionByFileTypeAsync(
                    DistributionFileType.GARFIASXmlDelta,
                    tempFileToDownload);
            }

            Assert.NotNull(lastInfo);
            Assert.True(File.Exists(tempFileToDownload));
            Assert.True(ZipHelper.IsZipValid(tempFileToDownload));
        }
    }
}
