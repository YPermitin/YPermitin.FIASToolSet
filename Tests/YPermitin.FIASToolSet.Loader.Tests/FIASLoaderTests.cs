namespace YPermitin.FIASToolSet.Loader.Tests
{
    public class FIASLoaderTests
    {
        [Fact]
        public async Task GetLastDistributionInfoTest()
        {
            IFIASLoader loader = FIASLoader.Create();
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
            IFIASLoader loader = FIASLoader.Create();
            var allInfo = await loader.GetAllDistributionInfo();

            Assert.NotNull(allInfo);
            Assert.True(allInfo.Count > 0);
        }
    }
}
