using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.API.Controllers
{
    /// <summary>
    /// Работа с актуальной информацией о ФИАС.
    /// Получается напрямую от ФНС и может быть использована для проверки актуальности ФИАС.
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Браузер дистрибутивов")]
    [ApiController]
    [Route("FIAS/actualDistribution")]
    public class FIASActualDistributionController : ControllerBase
    {
        private readonly IFIASDistributionBrowser _fiasDistributionBrowser;

        public FIASActualDistributionController(IFIASDistributionBrowser fiasDistributionBrowser)
        {
            _fiasDistributionBrowser = fiasDistributionBrowser;
        }

        [HttpGet(Name = "GetFIASActualDistributionInfo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FIASDistributionInfo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASActualDistributionInfo()
        {
            var fiasActualDistributionInfo = await _fiasDistributionBrowser.GetLastDistributionInfo();

            return Ok(fiasActualDistributionInfo);
        }
    }
}