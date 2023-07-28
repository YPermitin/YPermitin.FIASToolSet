using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage.BaseCatalogs
{
    /// <summary>
    /// Работа с типами помещений ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Базовые справочники", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/baseCatalog/apartmentTypes")]
    public class FIASApartmentTypeController : ControllerBase
    {
        private readonly IFIASBaseCatalogsRepository _baseCatalogs;
        private readonly IMapper _mapper;

        public FIASApartmentTypeController(
            IFIASBaseCatalogsRepository baseCatalogs,
            IMapper mapper)
        {
            _baseCatalogs = baseCatalogs;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка типов помещений ФИАС
        /// </summary>
        /// <returns>Список типов помещений ФИАС</returns>
        [HttpGet(Name = "GetFIASApartmentTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.BaseCatalogs.ApartmentType>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASApartmentTypes()
        {
            var entities = await _baseCatalogs.GetApartmentTypes();
            var objects = _mapper.Map<List<Models.BaseCatalogs.ApartmentType>>(entities);
            
            return Ok(objects);
        }
    }
}