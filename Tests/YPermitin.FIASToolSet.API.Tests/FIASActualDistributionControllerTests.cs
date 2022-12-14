using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.API.Controllers.FNS;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.API.Tests
{
    public class FIASActualDistributionControllerTests
    {
        [Fact]
        public async Task GetFIASActualDistributionInfoTest()
        {
            var fiasLoader = new FIASDistributionBrowser();
            var controller = new FIASActualDistributionController(fiasLoader);

            var result = await controller.GetFIASActualDistributionInfo();

            Assert.NotNull(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(result);
            var objectResult = Assert.IsType<FIASDistributionInfo>(okRequestResult.Value);
            Assert.NotNull(objectResult);
        }
    }
}