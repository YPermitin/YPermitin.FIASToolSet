﻿using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public JobsInitializer(IJobsManager jobsManager,
            ILogger<JobsInitializer> logger,
            IConfiguration configuration)
        {
            _jobsManager = jobsManager;
            _jobsManager.InitDefaultScheduler();
            _logger = logger;
            _configuration = configuration;
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
            await _jobsManager.AddActualizeFIASVersionHistoryJob(
                _configuration.GetValue("Jobs:Schedules:ActualizeFIASVersionHistoryJob", "0 0/10 * * * ?"));
            _logger.LogInformation("Запущено задание актуализации истории версий ФИАС.");

            // Запускаем задание отправки уведомлений
            bool useNotifications = _configuration.GetValue("Jobs:EnableNotification", false);
            if (useNotifications)
            {
                await _jobsManager.AddSendNotificationsJob(
                    _configuration.GetValue("Jobs:Schedules:ActualizeFIASVersionHistoryJob", "0 0/1 * * * ?"));
                _logger.LogInformation("Запущено задание отправка уведомлений.");
            }
            
            // Запускаем задание установки и обновления дистрибутивов ФИАС
            await _jobsManager.AddInstallAndUpdateFIASJob(
                _configuration.GetValue("Jobs:Schedules:InstallAndUpdateFIASJob", "0 0/1 * * * ?"));
            _logger.LogInformation("Запущено задание установки и обновления дистрибутива ФИАС.");

            _logger.LogInformation("Окончание запуска заданий");
        }
    }
}
