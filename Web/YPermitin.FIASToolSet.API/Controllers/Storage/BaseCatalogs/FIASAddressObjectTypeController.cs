using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage.BaseCatalogs
{
    /// <summary>
    /// Работа с типами адресных объектов ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Базовые справочники", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/baseCatalog/addressObjectTypes")]
    public class FIASAddressObjectTypeController : ControllerBase
    {
        private readonly IFIASBaseCatalogsRepository _baseCatalogs;
        private readonly IMapper _mapper;

        public FIASAddressObjectTypeController(
            IFIASBaseCatalogsRepository baseCatalogs,
            IMapper mapper)
        {
            _baseCatalogs = baseCatalogs;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка типов адресных объектов ФИАС
        /// </summary>
        /// <returns>Список типов адресных объектов ФИАС</returns>
        [HttpGet(Name = "GetFIASAddressObjectTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.BaseCatalogs.AddressObjectType>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASAddressObjectTypes()
        {
            var entities = await _baseCatalogs.GetAddressObjectTypes();
            var objects = _mapper.Map<List<Models.BaseCatalogs.AddressObjectType>>(entities);
            
            return Ok(objects);
        }
    }
}