using Serilog;
using Serilog.Events;
using TaskService1;
using Topshelf;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Quartz", LogEventLevel.Warning)
    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/LOG_.log"), rollingInterval: RollingInterval.Day)
    .CreateLogger();

var rc = HostFactory.Run(X =>
{
    X.UseSerilog();
    X.SetDescription("定时执行任务");
    X.SetDisplayName("任务调度");
    X.SetServiceName("TaskService");
    X.Service<QuartzService>(Y =>
    {
        Y.ConstructUsing(service => new QuartzService());
        Y.WhenStarted((tc, th) => tc.Start(th));
        Y.WhenStopped((tc, th) => tc.Stop(th));
        Y.WhenContinued((tc, th) => tc.Continue(th));
        Y.WhenPaused((tc, th) => tc.Pause(th));
    });
    X.RunAsLocalSystem();
    X.StartAutomatically();
    X.EnableServiceRecovery(R =>
    {
        R.RestartService(1); // 设置服务失败后的操作（一分钟后重启）
        R.RestartService(3); // 设置服务失败后的操作（三分钟后重启）
    });
});

int exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
Environment.ExitCode = exitCode;