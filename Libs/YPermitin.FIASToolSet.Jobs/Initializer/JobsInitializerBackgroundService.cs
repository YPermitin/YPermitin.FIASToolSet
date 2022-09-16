using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    public class JobsInitializerBackgroundService : BackgroundService
    {
        private readonly ILogger<JobsInitializerBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandStateManager _commandStateManager;

        public JobsInitializerBackgroundService(
            ILogger<JobsInitializerBackgroundService> logger,
            IServiceProvider serviceProvider,
            ICommandStateManager commandStateManager)
        {
            _commandStateManager = commandStateManager;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Отложенный запуск заданий инициализации заданий
            await Task.Delay(60000, stoppingToken);

            _logger.LogInformation("Сервис обновления заданий.");

            await DoWorkAsync(stoppingToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Сервис обновления заданий.");

            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            do
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IJobsInitializerProcessingService scopedProcessingService =
                        scope.ServiceProvider.GetRequiredService<IJobsInitializerProcessingService>();

                    var lastAction = _commandStateManager.GetLastActionAndReset();

                    if (lastAction == CommandState.InitStart)
                    {
                        _logger.LogInformation("Первый запуск заданий при инициализации.");
                       await scopedProcessingService.StartOrRestartAllJob(stoppingToken);
                    }
                    else if (lastAction == CommandState.Restart)
                    {
                        await scopedProcessingService.StartOrRestartAllJob(stoppingToken);
                    }
                    else if (lastAction == CommandState.Stop)
                    {
                        await scopedProcessingService.StopAllJob(stoppingToken);
                    }
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
