using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Com.Scm.Quartz
{
    public class JobFactory : IJobFactory
    {
        private static IServiceScopeFactory _serviceProvider;

        public JobFactory(IServiceScopeFactory  serviceScopeFactory)
        {
            _serviceProvider = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var service =  _serviceProvider.CreateScope();
            return service.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            ((IDisposable)job)?.Dispose();
        }
    }
}
