using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage.BaseCatalogs
{
    /// <summary>
    /// Работа с уровнями объектов ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Базовые справочники", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/baseCatalog/objectLevels")]
    public class FIASObjectLevelController : ControllerBase
    {
        private readonly IFIASBaseCatalogsRepository _baseCatalogs;
        private readonly IMapper _mapper;

        public FIASObjectLevelController(
            IFIASBaseCatalogsRepository baseCatalogs,
            IMapper mapper)
        {
            _baseCatalogs = baseCatalogs;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка уровней адресных объектов ФИАС
        /// </summary>
        /// <returns>Список уровней адресных объектов ФИАС</returns>
        [HttpGet(Name = "GetFIASObjectLevels")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.BaseCatalogs.ObjectLevel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASObjectLevels()
        {
            var entities = await _baseCatalogs.GetObjectLevels();
            var objects = _mapper.Map<List<Models.BaseCatalogs.ObjectLevel>>(entities);
            
            return Ok(objects);
        }
    }
}