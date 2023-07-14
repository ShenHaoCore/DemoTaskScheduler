using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace TaskService2
{
    /// <summary>
    /// 周期性作业服务
    /// </summary>
    public class RecurringJobService
    {
        /// <summary>
        /// 简单作业
        /// </summary>
        /// <param name="context"></param>
        public async Task SimpleJob([FromResult] PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} SimpleJob Running ...");
            var progressBar = context.WriteProgressBar();
            foreach (var i in Enumerable.Range(1, 50).ToList().WithProgress(progressBar))
            {
                Thread.Sleep(100);
            }
            await Task.Yield();
        }
    }
}
