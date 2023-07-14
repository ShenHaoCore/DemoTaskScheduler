using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using Topshelf;

namespace TaskService3
{
    /// <summary>
    /// 
    /// </summary>
    internal class Program
    {
        private static readonly string baseAddress = "http://localhost:9000";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var log4netRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log4netRepository, new FileInfo("Log4Net.config"));

            TopshelfExitCode rc = HostFactory.Run(X =>
            {
                X.RunAsLocalSystem();
                //服务使用NETWORK_SERVICE内置帐户运行。
                //身份标识有好几种方式，如：X.RunAs("username", "password"); X.RunAsPrompt(); X.RunAsNetworkService(); 等

                X.UseOwin(baseAddress: baseAddress);

                X.SetDescription("自动执行作业(TaskService)");//安装服务后，服务的描述
                X.SetDisplayName("任务服务");//显示名称
                X.SetServiceName("TaskService");//服务名称

                //设置服务失败后的操作
                X.EnableServiceRecovery(r =>
                {
                    r.RestartService(1); //等待延迟时间（分钟）后重新启动服务
                });
            });

            int exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
