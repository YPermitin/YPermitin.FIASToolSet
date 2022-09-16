using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using YPermitin.FIASToolSet.Jobs.Extensions;
using YPermitin.FIASToolSet.Jobs.Models;

// ReSharper disable NotAccessedField.Local

namespace YPermitin.FIASToolSet.Jobs
{
    public class QuartzHostedService : IHostedService, IJobsManager
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private readonly IConfiguration _configuration;

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules,
            IConfiguration configuration)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _configuration = configuration;
            _jobFactory = jobFactory;
        }
        public IScheduler Scheduler { get; set; }

        public async Task InitDefaultScheduler()
        {
            if (Scheduler == null)
            {
                var q = SchedulerBuilder.Create();

                // Идентификатор планировщика
                q.SchedulerId = "Scheduler-Core";

                // Добавляем контроль прерывания заданий
                q.InterruptJobsOnShutdown = true;
                // Ожидаем завершения задания
                q.InterruptJobsOnShutdownWithWait = true;

                // Максимальное количество заданий для одновременного выполнения
                int maxBatchSize = _configuration.GetValue("Jobs:MaxBatchSize", 0);
                q.MaxBatchSize = maxBatchSize <= 0 ? 10 : maxBatchSize;

                // Включаем интеграцию с Microsoft.DI
                //q.UseMicrosoftDependencyInjectionJobFactory();

                // Дополнительные настройки места хранения, потоков и т.д.
                //q.UseSimpleTypeLoader();
                //q.UseInMemoryStore();
                int maxThreadPoolConcurrency = _configuration.GetValue("Jobs:ThreadPoolConcurrency", 0);
                q.UseDefaultThreadPool(maxConcurrency: maxThreadPoolConcurrency <= 0 ? 10 : maxThreadPoolConcurrency);

                Scheduler = await q.BuildScheduler();

                //Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                Scheduler.JobFactory = _jobFactory;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitDefaultScheduler();

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler!.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler!.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken)!;
        }

        public async Task<IReadOnlyCollection<JobInfo>> GetAllJobs(bool activeOnly = false)
        {
            return await Scheduler!.GetAllJobs(activeOnly);
        }

        public async Task CreateJobItem<T>(JobInfoWithDetails jobInfo)
            where T : IJob
        {
            await CreateJobItem<T>(jobInfo, CancellationToken.None);
        }

        public async Task CreateJobItem<T>(JobInfoWithDetails jobInfo, CancellationToken cancellationToken)
            where T : IJob
        {
            if (string.IsNullOrEmpty(jobInfo.Key))
                jobInfo.Key = Guid.NewGuid().ToString();

            var job = CreateJob<T>(jobInfo);
            var trigger = CreateTrigger(jobInfo);

            await Scheduler!.ScheduleJob(job, trigger, cancellationToken);
        }

        public async Task UpdateJobItem<T>(JobInfoWithDetails jobInfo)
            where T : IJob
        {
            await UpdateJobItem<T>(jobInfo, CancellationToken.None);
        }

        public async Task UpdateJobItem<T>(JobInfoWithDetails jobInfo, CancellationToken cancellationToken)
            where T : IJob
        {
            var allTriggerKeys = await Scheduler!.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup(), cancellationToken);
            foreach (var triggerKey in allTriggerKeys)
            {
                if (triggerKey.Name == $"{jobInfo.Key}.trigger")
                {
                    await Scheduler.UnscheduleJob(triggerKey, cancellationToken);
                    await CreateJobItem<T>(jobInfo, cancellationToken);
                }
            }
        }

        public async Task DeleteJobItem(string jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            var allTriggerKeys = await Scheduler!.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup(), CancellationToken.None);
            foreach (var triggerKey in allTriggerKeys)
            {
                if (triggerKey.Name == $"{jobKey}.trigger")
                {
                    await Scheduler.UnscheduleJob(triggerKey, cancellationToken);
                }
            }
        }

        public async Task InterruptJobItem(string jobKeyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jobKey = await Scheduler.GetJobKeyByName(jobKeyName);
            if(jobKey != null)
                await Scheduler.Interrupt(jobKey, cancellationToken);
        }

        #region Services

        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName!)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static IJobDetail CreateJob<T>(JobInfoWithDetails jobInfo)
            where T : IJob
        {
            string jsonParameterAsJson = JsonSerializer.Serialize(jobInfo.Settings);

            var jobType = typeof(T);
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap.Add("Parameter", jsonParameterAsJson);

            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobInfo.Key)
                .WithDescription(jobInfo.Description)
                .SetJobData(jobDataMap)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }

        private static ITrigger CreateTrigger(JobInfoWithDetails jobInfo)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{jobInfo.Key}.trigger")
                .WithCronSchedule(jobInfo.CronExpression)
                .WithDescription($"{jobInfo.Description} ({jobInfo.CronExpression})")
                .WithPriority(jobInfo.Priority)
                .Build();
        }

        #endregion
    }
}
