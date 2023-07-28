using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage.BaseCatalogs
{
    /// <summary>
    /// Работа с типами операций ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Базовые справочники", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/baseCatalog/operationTypes")]
    public class FIASOperationTypeController : ControllerBase
    {
        private readonly IFIASBaseCatalogsRepository _baseCatalogs;
        private readonly IMapper _mapper;

        public FIASOperationTypeController(
            IFIASBaseCatalogsRepository baseCatalogs,
            IMapper mapper)
        {
            _baseCatalogs = baseCatalogs;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка типов операций ФИАС
        /// </summary>
        /// <returns>Список типов операций ФИАС</returns>
        [HttpGet(Name = "GetFIASOperationTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.BaseCatalogs.OperationType>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASOperationTypes()
        {
            var entities = await _baseCatalogs.GetOperationTypes();
            var objects = _mapper.Map<List<Models.BaseCatalogs.OperationType>>(entities);
            
            return Ok(objects);
        }
    }
}