using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace YPermitin.FIASToolSet.Jobs
{
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob 
                   ?? throw new InvalidOperationException();
        }

        public void ReturnJob(IJob job) { }
    }
}
