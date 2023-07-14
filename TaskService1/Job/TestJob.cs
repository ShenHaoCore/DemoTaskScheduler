using Microsoft.Extensions.Logging;
using Quartz;

namespace TaskService1
{
    /// <summary>
    /// 
    /// </summary>
    public class TestJob : IJob
    {
        private readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public TestJob(ILogger<TestJob> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation(string.Format("[{0:yyyy-MM-dd HH:mm:ss:fff}]任务开始执行！", DateTime.Now));
            return Task.CompletedTask;
        }
    }
}
