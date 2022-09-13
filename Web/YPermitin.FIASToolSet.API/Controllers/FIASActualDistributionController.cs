using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Loader;
using YPermitin.FIASToolSet.Loader.Models;

namespace YPermitin.FIASToolSet.API.Controllers
{
    /// <summary>
    /// Работа с актуальной информацией о ФИАС.
    /// Получается напрямую от ФНС и может быть использована для проверки актуальности ФИАС.
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Актуальная информация")]
    [ApiController]
    [Route("FIAS/actualDistribution")]
    public class FIASActualDistributionController : ControllerBase
    {
        private readonly IFIASLoader _fiasLoader;

        public FIASActualDistributionController(IFIASLoader fiasLoader)
        {
            _fiasLoader = fiasLoader;
        }

        [HttpGet(Name = "GetFIASActualDistributionInfo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FIASDistributionInfo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASActualDistributionInfo()
        {
            var fiasActualDistributionInfo = await _fiasLoader.GetLastDistributionInfo();

            return Ok(fiasActualDistributionInfo);
        }
    }
}