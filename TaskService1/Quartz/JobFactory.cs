using Quartz;
using Quartz.Spi;

namespace TaskService1
{
    /// <summary>
    /// 
    /// </summary>
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceProvider provider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provider"></param>
        public JobFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = provider.GetService(bundle.JobDetail.JobType) as IJob;
            ArgumentNullException.ThrowIfNull(job);
            return job!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}
