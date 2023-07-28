using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage.BaseCatalogs
{
    /// <summary>
    /// Работа с типами нормативных документов ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Базовые справочники", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/baseCatalog/normativeDocTypes")]
    public class FIASNormativeDocTypeController : ControllerBase
    {
        private readonly IFIASBaseCatalogsRepository _baseCatalogs;
        private readonly IMapper _mapper;

        public FIASNormativeDocTypeController(
            IFIASBaseCatalogsRepository baseCatalogs,
            IMapper mapper)
        {
            _baseCatalogs = baseCatalogs;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка типов нормативных документов ФИАС
        /// </summary>
        /// <returns>Список типов нормативных документов ФИАС</returns>
        [HttpGet(Name = "GetFIASNormativeDocTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.BaseCatalogs.NormativeDocType>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASNormativeDocTypes()
        {
            var entities = await _baseCatalogs.GetNormativeDocTypes();
            var objects = _mapper.Map<List<Models.BaseCatalogs.NormativeDocType>>(entities);
            
            return Ok(objects);
        }
    }
}