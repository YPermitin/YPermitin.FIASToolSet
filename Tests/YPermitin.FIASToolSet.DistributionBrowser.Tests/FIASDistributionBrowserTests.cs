using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionBrowser.Helpers;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

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
            var customWorkingDirectory = Path.Combine(
                Path.GetTempPath(),
                "FIASToolSet_Tests_DownloadLastGarXmlDeltaDistribution");
            
            IFIASDistributionBrowser loader = new FIASDistributionBrowser(
                new FIASDistributionBrowserOptions(customWorkingDirectory));
            var lastInfo = await loader.GetLastDistributionInfo();

            string downloadedFilePath = null;
            if (lastInfo != null)
            {
                var fileType = DistributionFileType.GARFIASXmlDelta;
                await lastInfo.DownloadDistributionByFileTypeAsync(fileType);
                downloadedFilePath = lastInfo.GetLocalPathByFileType(fileType);
            }

            Assert.NotNull(lastInfo);
            Assert.NotNull(downloadedFilePath);
            Assert.True(File.Exists(downloadedFilePath));
            Assert.True(ZipHelper.IsZipValid(downloadedFilePath));

            lastInfo?.RemoveVersionWorkingDirectory();
            
            loader.ClearAllWorkingDirectories();
            Assert.True(!Directory.Exists(loader.Options.WorkingDirectory));
        }
        
        [Fact]
        public async Task ExtractLastGarXmlDeltaDistribution()
        {
            var customWorkingDirectory = Path.Combine(
                Path.GetTempPath(),
                "FIASToolSet_Tests_ExtractLastGarXmlDeltaDistribution");
            
            IFIASDistributionBrowser loader = new FIASDistributionBrowser(
                new FIASDistributionBrowserOptions(customWorkingDirectory));
            var lastInfo = await loader.GetLastDistributionInfo();

            string downloadedFilePath = null;
            string extractedDirectory = null;
            if (lastInfo != null)
            {
                var fileType = DistributionFileType.GARFIASXmlDelta;
                await lastInfo.DownloadDistributionByFileTypeAsync(fileType);
                downloadedFilePath = lastInfo.GetLocalPathByFileType(fileType);
                
                lastInfo.ExtractDistributionFile(fileType);
                extractedDirectory = lastInfo.GetExtractedDirectory(fileType);
            }


            Assert.NotNull(lastInfo);
            Assert.NotNull(downloadedFilePath);
            Assert.True(File.Exists(downloadedFilePath));
            Assert.True(ZipHelper.IsZipValid(downloadedFilePath));
            Assert.True(Directory.Exists(extractedDirectory));
            Assert.True(Directory.GetFiles(extractedDirectory, "*.*", SearchOption.AllDirectories).Length > 0);

            lastInfo?.RemoveVersionWorkingDirectory();
            Assert.True(!Directory.Exists(extractedDirectory));
            
            loader.ClearAllWorkingDirectories();
            Assert.True(!Directory.Exists(loader.Options.WorkingDirectory));
        }
        
        [Fact]
        public async Task ExtractLastGarXmlDeltaDistributionWithRegionFilter()
        {
            var customWorkingDirectory = Path.Combine(
                Path.GetTempPath(),
                "FIASToolSet_Tests_ExtractLastGarXmlDeltaDistributionWithRegionFilter");
            
            IFIASDistributionBrowser loader = new FIASDistributionBrowser(
                new FIASDistributionBrowserOptions(customWorkingDirectory));
            var lastInfo = await loader.GetLastDistributionInfo();

            string downloadedFilePath = null;
            string extractedDirectory = null;
            if (lastInfo != null)
            {
                var fileType = DistributionFileType.GARFIASXmlDelta;
                await lastInfo.DownloadDistributionByFileTypeAsync(fileType);
                downloadedFilePath = lastInfo.GetLocalPathByFileType(fileType);

                var regionsInFile = lastInfo.GetAvailableRegions(fileType);

                string[] regionFilter = null;
                if (regionsInFile.Length > 0)
                {
                    regionFilter = new string[1];
                    regionFilter[0] = regionsInFile[0];
                }
                lastInfo.ExtractDistributionFile(fileType, 
                    regions: regionFilter);
                extractedDirectory = lastInfo.GetExtractedDirectory(fileType);
            }


            Assert.NotNull(lastInfo);
            Assert.NotNull(downloadedFilePath);
            Assert.True(File.Exists(downloadedFilePath));
            Assert.True(ZipHelper.IsZipValid(downloadedFilePath));
            Assert.True(Directory.Exists(extractedDirectory));
            Assert.True(Directory.GetFiles(extractedDirectory, "*.*", SearchOption.AllDirectories).Length > 0);
            Assert.Single(Directory.GetDirectories(extractedDirectory));
            
            lastInfo?.RemoveVersionWorkingDirectory();
            Assert.True(!Directory.Exists(extractedDirectory));

            loader.ClearAllWorkingDirectories();
            Assert.True(!Directory.Exists(loader.Options.WorkingDirectory));
        }
        
        [Fact]
        public async Task ExtractLastGarXmlDeltaDistributionWithBaseFilesFilter()
        {
            var customWorkingDirectory = Path.Combine(
                Path.GetTempPath(),
                "FIASToolSet_Tests_ExtractLastGarXmlDeltaDistributionWithBaseFilesFilter");
            
            IFIASDistributionBrowser loader = new FIASDistributionBrowser(
                new FIASDistributionBrowserOptions(customWorkingDirectory));
            var lastInfo = await loader.GetLastDistributionInfo();

            string downloadedFilePath = null;
            string extractedDirectory = null;
            if (lastInfo != null)
            {
                var fileType = DistributionFileType.GARFIASXmlDelta;
                await lastInfo.DownloadDistributionByFileTypeAsync(fileType);
                downloadedFilePath = lastInfo.GetLocalPathByFileType(fileType);
                
                lastInfo.ExtractDistributionFile(fileType, 
                    onlyBaseFiles: true);
                extractedDirectory = lastInfo.GetExtractedDirectory(fileType);
            }


            Assert.NotNull(lastInfo);
            Assert.NotNull(downloadedFilePath);
            Assert.True(File.Exists(downloadedFilePath));
            Assert.True(ZipHelper.IsZipValid(downloadedFilePath));
            Assert.True(Directory.Exists(extractedDirectory));
            Assert.True(Directory.GetFiles(extractedDirectory, "*.*", SearchOption.AllDirectories).Length > 0);
            Assert.Empty(Directory.GetDirectories(extractedDirectory));
            
            lastInfo?.RemoveVersionWorkingDirectory();
            Assert.True(!Directory.Exists(extractedDirectory));

            loader.ClearAllWorkingDirectories();
            Assert.True(!Directory.Exists(loader.Options.WorkingDirectory));
        }
    }
}
