using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService3
{
    /// <summary>
    /// 定时作业服务
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
                System.Threading.Thread.Sleep(1000);
            }
            await Task.Yield();
        }
    }
}
