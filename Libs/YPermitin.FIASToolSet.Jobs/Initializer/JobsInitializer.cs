using Microsoft.Extensions.Logging;
using YPermitin.FIASToolSet.Jobs.Extensions;

namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    /// <summary>
    /// Объет инициализации заданий
    /// </summary>
    public class JobsInitializer : IJobsInitializer
    {
        private readonly IJobsManager _jobsManager;
        private readonly ILogger<JobsInitializer> _logger;

        public JobsInitializer(IJobsManager jobsManager,
            ILogger<JobsInitializer> logger)
        {
            _jobsManager = jobsManager;
            _jobsManager.InitDefaultScheduler();
            _logger = logger;
        }

        /// <summary>
        /// Остановить задания
        /// </summary>
        /// <returns>Объект асинхронной операции</returns>
        public async Task StopAllJob()
        {
            _logger.LogInformation("Начало остановки всех заданий");
            
            var allJobs = await _jobsManager.GetAllJobs();
            foreach (var jobItem in allJobs)
            {
                await _jobsManager.InterruptJobItem(jobItem.JobKey);
                await _jobsManager.DeleteJobItem(jobItem.JobKey);
                _logger.LogInformation($"Задание остановлено: {jobItem.JobKey} - {jobItem.JobName}");
            }

            var activeJobs = await _jobsManager.GetAllJobs(true);
            if (activeJobs.Count > 0)
            {
                while (activeJobs.Count > 0)
                {
                    _logger.LogInformation($"Ожидание завершения активных заданий. Осталось: {activeJobs.Count}");
                    await Task.Delay(1000);
                    activeJobs = await _jobsManager.GetAllJobs(true);
                }
            }

            _logger.LogInformation("Окончание остановки всех заданий");
        }

        /// <summary>
        /// Запуск / перезаупск заданий
        /// </summary>
        /// <returns>Объект асинхронной операции</returns>
        public async Task StartOrRestartAllJob()
        {
            // Останавливаем все задания
            await StopAllJob();

            _logger.LogInformation("Начало запуска заданий");

            var activeJobs = await _jobsManager.GetAllJobs(true);
            while (activeJobs.Count > 0)
            {
                _logger.LogInformation($"Ожидание завершения активных заданий. Осталось: {activeJobs.Count}");
                await Task.Delay(1000);
                activeJobs = await _jobsManager.GetAllJobs(true);
            }
            
            // Запускаем задание актуализации истории версий ФИАС
            await _jobsManager.AddActualizeFIASVersionHistoryJob("0 0/10 * * * ?");
            _logger.LogInformation("Запущено задание актуализации истории версий ФИАС.");

            // Запускаем задание отправки уведомлений
            await _jobsManager.AddSendNotificationsJob("0 0/1 * * * ?");
            _logger.LogInformation("Запущено задание отправка уведомлений.");

            _logger.LogInformation("Окончание запуска заданий");
        }
    }
}
