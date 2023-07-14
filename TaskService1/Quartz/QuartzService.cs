using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using Topshelf;

namespace TaskService1
{
    /// <summary>
    /// Quartz服务
    /// </summary>
    public class QuartzService : ServiceControl, ServiceSuspend
    {
        private IScheduler? scheduler;

        /// <summary>
        /// 服务启动
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(HostControl hostControl)
        {
            try
            {
                Log.Information("服务正在启动...");
                ServiceProvider provider = Startup.ConfigureServices();
                var scheduler = provider.GetService(typeof(IScheduler)) as IScheduler;
                ArgumentNullException.ThrowIfNull(scheduler);
                this.scheduler = scheduler;
                scheduler.Start();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("服务启动失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            try
            {
                Log.Information("服务正在停止...");
                ArgumentNullException.ThrowIfNull(scheduler);
                scheduler.Shutdown();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("服务停止失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Continue(HostControl hostControl)
        {
            try
            {
                Log.Information("服务正在恢复...");
                ArgumentNullException.ThrowIfNull(scheduler);
                scheduler.ResumeAll();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("服务恢复失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Pause(HostControl hostControl)
        {
            try
            {
                Log.Information("服务正在暂停...");
                ArgumentNullException.ThrowIfNull(scheduler);
                scheduler.PauseAll();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("服务暂停失败", ex);
                return false;
            }
        }
    }
}
