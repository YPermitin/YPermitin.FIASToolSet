namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    public interface IJobsInitializerProcessingService
    {
        Task StartOrRestartAllJob(CancellationToken cancellationToken);
        Task StopAllJob(CancellationToken cancellationToken);
    }
}
