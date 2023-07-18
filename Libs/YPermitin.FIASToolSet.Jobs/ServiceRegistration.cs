using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using YPermitin.FIASToolSet.Jobs.Extensions;
using YPermitin.FIASToolSet.Jobs.Initializer;
using YPermitin.FIASToolSet.Jobs.JobItems;

namespace YPermitin.FIASToolSet.Jobs
{
    public static class ServiceRegistration
    {
        public static void AddJobsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzHostedService>();

            // Расписание заданий настраивается индивидуально при добавлении
            services.RegisterJob<ActualizeFIASVersionHistoryJob>();
            services.RegisterJob<InstallAndUpdateFIASJob>();
            services.RegisterJob<SendNotificationsJob>();

            var serviceProvider = services.BuildServiceProvider();
            var quartzHostedService = serviceProvider.GetRequiredService<QuartzHostedService>();

            services.AddHostedService(_ => quartzHostedService);
            services.AddSingleton<IJobsManager>(_ => quartzHostedService);

            services.AddScoped<IJobsInitializer, JobsInitializer>();

            // Добавление сервисов асинхронного управления заданиями
            // Данный сервис делает начальный запуск заданий, а после управляет командами остановки или перезапуска
            services.AddSingleton<ICommandStateManager, CommandStateManager>();
            services.AddScoped<IJobsInitializerProcessingService, JobsInitializerProcessingService>();
            services.AddHostedService<JobsInitializerBackgroundService>();
        }
    }
}
