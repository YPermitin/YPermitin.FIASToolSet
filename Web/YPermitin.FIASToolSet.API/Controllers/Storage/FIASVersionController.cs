using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.API.Models.Versions;
using YPermitin.FIASToolSet.Storage.Core.Services;

namespace YPermitin.FIASToolSet.API.Controllers.Storage
{
    /// <summary>
    /// Информация о версиях ФИАС
    /// </summary>
    [ApiExplorerSettings(GroupName = "ФИАС. Управление хранилищем", IgnoreApi = false)]
    [ApiController]
    [Route("FIAS/storage/versions")]
    public class FIASVersionController : ControllerBase
    {
        private readonly IFIASMaintenanceRepository _fiasMaintenanceService;
        private readonly IMapper _mapper;

        public FIASVersionController(
            IFIASMaintenanceRepository fiasMaintenanceService,
            IMapper mapper)
        {
            _fiasMaintenanceService = fiasMaintenanceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение последней версии ФИАС, сохраненной в хранилище сервиса
        /// </summary>
        /// <returns>Информация о последней версии ФИАС</returns>
        [HttpGet(Name = "GetFIASLastVersion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FIASVersion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetFIASLastVersion()
        {
            var lastVersionEntity = await _fiasMaintenanceService.GetLastVersion();
            if (lastVersionEntity == null)
            {
                return BadRequest(
                    "Не удалось найти ни одной версии ФИАС. " +
                    "Возможно, не была выполнена настройка сервиса для получения данных с API ФИАС.");
            }

            // Последняя версия из хранилища
            var lastVersion = _mapper.Map<FIASVersion>(lastVersionEntity);
            
            return Ok(lastVersion);
        }
    }
}