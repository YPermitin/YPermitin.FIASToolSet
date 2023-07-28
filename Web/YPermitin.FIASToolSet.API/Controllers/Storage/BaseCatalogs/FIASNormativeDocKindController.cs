using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage.BaseCatalogs
{
    /// <summary>
    /// Работа с видами нормативных документов ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Базовые справочники", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/baseCatalog/normativeDocKinds")]
    public class FIASNormativeDocKindController : ControllerBase
    {
        private readonly IFIASBaseCatalogsRepository _baseCatalogs;
        private readonly IMapper _mapper;

        public FIASNormativeDocKindController(
            IFIASBaseCatalogsRepository baseCatalogs,
            IMapper mapper)
        {
            _baseCatalogs = baseCatalogs;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка видов нормативных документов ФИАС
        /// </summary>
        /// <returns>Список видов нормативных документов ФИАС</returns>
        [HttpGet(Name = "GetFIASNormativeDocKinds")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.BaseCatalogs.NormativeDocKind>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASNormativeDocKinds()
        {
            var entities = await _baseCatalogs.GetNormativeDocKinds();
            var objects = _mapper.Map<List<Models.BaseCatalogs.NormativeDocKind>>(entities);
            
            return Ok(objects);
        }
    }
}