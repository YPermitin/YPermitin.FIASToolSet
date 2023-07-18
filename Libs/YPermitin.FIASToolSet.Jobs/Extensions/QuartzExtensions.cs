using Quartz;
using Quartz.Impl.Matchers;
using YPermitin.FIASToolSet.Jobs.JobItems;
using YPermitin.FIASToolSet.Jobs.Models;

namespace YPermitin.FIASToolSet.Jobs.Extensions
{
    /// <summary>
    /// Расширение работы с планировщиком заданий
    /// </summary>
    public static class QuartzExtensions
    {
        /// <summary>
        /// Формирование списка заданий
        /// </summary>
        /// <param name="scheduler">Объект планировщика</param>
        /// <param name="activeOnly">Только активные задания</param>
        /// <returns>Список заданий с доп. атрибутами</returns>
        public static async Task<IReadOnlyCollection<JobInfo>> GetAllJobs(this IScheduler scheduler, bool activeOnly = false)
        {
            Dictionary<string, JobInfo> allJobsDict = new Dictionary<string, JobInfo>();

            var activeJobs = await scheduler.GetCurrentlyExecutingJobs(CancellationToken.None);
            foreach (var activeJob in activeJobs)
            {
                var jobTrigger = activeJob.Trigger;
                string cronExpression = string.Empty;
                if (jobTrigger is Quartz.Impl.Triggers.CronTriggerImpl)
                {
                    cronExpression = ((Quartz.Impl.Triggers.CronTriggerImpl)jobTrigger).CronExpressionString ??
                                     string.Empty;
                }

                string jobKeyNormalized = jobTrigger.JobKey.Name.ToUpper().Trim();
                if (!allJobsDict.ContainsKey(jobKeyNormalized))
                {
                    allJobsDict.Add(jobKeyNormalized, new JobInfo()
                    {
                        JobKey = jobTrigger.JobKey.Name,
                        JobName = jobTrigger.Description ?? string.Empty,
                        CronExpression = cronExpression,
                        PreviousFireTime = jobTrigger.GetPreviousFireTimeUtc(),
                        NextFireTime = jobTrigger.GetNextFireTimeUtc(),
                        Status = "Active"
                    });
                }
            }

            if (!activeOnly)
            {
                // Добавляем задания, которые не активные в данный момент
                IReadOnlyCollection<string> jobGroups = await scheduler.GetJobGroupNames();
                foreach (string group in jobGroups)
                {
                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                    var jobKeys = await scheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        var triggers = await scheduler.GetTriggersOfJob(jobKey);
                        foreach (ITrigger trigger in triggers)
                        {
                            string cronExpression =
                                ((Quartz.Impl.Triggers.CronTriggerImpl)trigger).CronExpressionString ?? string.Empty;

                            string jobKeyNormalized = trigger.JobKey.Name.ToUpper().Trim();
                            if (!allJobsDict.ContainsKey(jobKeyNormalized))
                            {
                                allJobsDict.Add(jobKeyNormalized, new JobInfo()
                                {
                                    JobKey = trigger.JobKey.Name,
                                    JobName = trigger.Description ?? string.Empty,
                                    CronExpression = cronExpression,
                                    PreviousFireTime = trigger.GetPreviousFireTimeUtc(),
                                    NextFireTime = trigger.GetNextFireTimeUtc(),
                                    Status = "Idle"
                                });
                            }
                        }
                    }
                }
            }

            return allJobsDict.Select(e => e.Value).ToList();
        }

        /// <summary>
        /// Поиск задания по имени
        /// </summary>
        /// <param name="scheduler">Объект планировщика</param>
        /// <param name="jobKeyName">Наименование задания</param>
        /// <returns>Объект задания</returns>
        public static async Task<JobKey> GetJobKeyByName(this IScheduler scheduler, string jobKeyName)
        {
            IReadOnlyCollection<string> jobGroups = await scheduler.GetJobGroupNames();
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = await scheduler.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var triggers = await scheduler.GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        if (trigger.JobKey.Name == jobKeyName)
                        {
                            return jobKey;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Регистрация задания актуализации списка версий ФИАС.
        /// </summary>
        /// <param name="jobsManager">Объект планировщика</param>
        /// <param name="cronExpression">CRON-выражение запуска задания</param>
        /// <returns>Объект асинхронной операции</returns>
        public static async Task AddActualizeFIASVersionHistoryJob(this IJobsManager jobsManager, string cronExpression)
        {
            var jobInfo = new JobInfoWithDetails
            {
                Key = Guid.NewGuid().ToString(),
                Description = "Actualize FIAS version history",
                CronExpression = cronExpression,
                Priority = 1,
                Settings = new JobSettings()
            };

            await jobsManager.CreateJobItem<ActualizeFIASVersionHistoryJob>(jobInfo, CancellationToken.None);
        }

        /// <summary>
        /// Регистрация задания отправки уведомлений
        /// </summary>
        /// <param name="jobsManager">Объект планировщика</param>
        /// <param name="cronExpression">CRON-выражение запуска задания</param>
        /// <returns>Объект асинхронной операции</returns>
        public static async Task AddSendNotificationsJob(this IJobsManager jobsManager, string cronExpression)
        {
            var jobInfo = new JobInfoWithDetails
            {
                Key = Guid.NewGuid().ToString(),
                Description = "Send notifications",
                CronExpression = cronExpression,
                Priority = 1,
                Settings = new JobSettings()
            };

            await jobsManager.CreateJobItem<SendNotificationsJob>(jobInfo, CancellationToken.None);
        }
        
        /// <summary>
        /// Регистрация задания установки и обновления дистрибутивов ФИАС.
        /// </summary>
        /// <param name="jobsManager">Объект планировщика</param>
        /// <param name="cronExpression">CRON-выражение запуска задания</param>
        /// <returns>Объект асинхронной операции</returns>
        public static async Task AddInstallAndUpdateFIASJob(this IJobsManager jobsManager, string cronExpression)
        {
            var jobInfo = new JobInfoWithDetails
            {
                Key = Guid.NewGuid().ToString(),
                Description = "Install or update FIAS distribution",
                CronExpression = cronExpression,
                Priority = 1,
                Settings = new JobSettings()
            };

            await jobsManager.CreateJobItem<InstallAndUpdateFIASJob>(jobInfo, CancellationToken.None);
        }
    }
}
