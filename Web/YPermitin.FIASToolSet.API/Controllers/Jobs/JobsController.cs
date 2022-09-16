using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using YPermitin.FIASToolSet.API.Extensions;
using YPermitin.FIASToolSet.Jobs;
using YPermitin.FIASToolSet.Jobs.Initializer;
using YPermitin.FIASToolSet.Jobs.Models;

namespace YPermitin.FIASToolSet.API.Controllers.Jobs
{
    [ApiController]
    [Route("jobs/[action]")]
    [ApiExplorerSettings(GroupName = "Планировщик заданий", IgnoreApi = true)]
    public class JobsController : ControllerBase
    {
        private readonly IJobsManager _jobsManager;
        private readonly ICommandStateManager _commandStateManager;

        public JobsController(IJobsManager jobsManager,
            ICommandStateManager commandStateManager)
        {
            _jobsManager = jobsManager;
            _commandStateManager = commandStateManager;
        }

        /// <summary>
        /// Формирование списка заданий
        /// </summary>
        /// <returns>Список заданий</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<JobInfo>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ActionName("list")]
        [HttpGet(Name = "GetAllJobs")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllJobs()
        {
            if (!HttpContext.IsLocal())
                return Unauthorized();

            var allJobs = await _jobsManager.GetAllJobs();

            return Ok(allJobs);
        }

        /// <summary>
        /// Остановка всех заданий
        /// </summary>
        /// <returns>Пустой ответ в случае успеха</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ActionName("stopAll")]
        [HttpPost(Name = "StopAllJobs")]        
        public IActionResult StopAllJobs()
        {
            if (!HttpContext.IsLocal())
                return Unauthorized();

            _commandStateManager.SetStop();

            return NoContent();
        }

        /// <summary>
        /// Запуск / перезапуск (если уже были запущены) всех заданий
        /// </summary>
        /// <returns>Пустой ответ в случае успеха</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ActionName("restartOrStartAll")]
        [HttpPost(Name = "RestartOrStartAll")]
        public IActionResult RestartAllJobs()
        {
            if (!HttpContext.IsLocal())
                return Unauthorized();

            _commandStateManager.SetRestart();

            return NoContent();
        }
    }
}
