using Microsoft.Extensions.Logging;

namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    public class JobsInitializerProcessingService : IJobsInitializerProcessingService
    {
        private readonly ILogger<JobsInitializerProcessingService> _logger;
        private readonly IJobsInitializer _jobsInitializer;

        public JobsInitializerProcessingService(ILogger<JobsInitializerProcessingService> logger,
            IJobsInitializer jobsInitializer)
        {
            _logger = logger;
            _jobsInitializer = jobsInitializer;
        }

        public async Task StartOrRestartAllJob(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Начало перезапуска заданий.");

            await _jobsInitializer.StartOrRestartAllJob();

            _logger.LogInformation("Окончание перезапуска заданий.");
        }

        public async Task StopAllJob(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Начало остановки заданий.");

            await _jobsInitializer.StopAllJob();

            _logger.LogInformation("Окончание остановки заданий.");
        }
    }
}
