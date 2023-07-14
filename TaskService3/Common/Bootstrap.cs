using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;
using Topshelf;
using Topshelf.Logging;

namespace TaskService3
{
    /// <summary>
    /// 引导程序
    /// </summary>
    public class Bootstrap : ServiceControl
    {
        private readonly LogWriter _logger = HostLogger.Get(typeof(Bootstrap));
        private IDisposable webApp;

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(HostControl hostControl)
        {
            try
            {
                webApp = WebApp.Start<Startup>(Address);

                #region 打印信息
                _logger.Info("任务服务启动中...");
                _logger.Info(string.Format("主页：{0}", Address));

                //_logger.Info(string.Format("控制台：{0}/Hangfire", Address));
                //_logger.Info(string.Format("WebAPI：{0}/swagger/ui/index", Address));
                #endregion

#if DEBUG
                Process.Start($@"{Address}");

                //Process.Start($@"{Address}/Hangfire");
                //Process.Start($@"{Address}/swagger/ui/index");
#endif

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Topshelf 启动发生的错误:{0}", ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            try
            {
                _logger.Info("任务服务停止中...");
                webApp?.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Topshelf 停止发生的错误:{0}", ex.ToString()));
                return false;
            }
        }
    }
}
