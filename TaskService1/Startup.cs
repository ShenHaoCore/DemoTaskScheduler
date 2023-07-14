using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;

namespace TaskService1
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        public static ServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var scheduler = new StdSchedulerFactory(new QuartzOption(config).ToProperties()).GetScheduler().Result;

            IServiceCollection services = new ServiceCollection();
            services.AddOptions();
            services.AddLogging(log => log.AddSerilog(dispose: true));
            services.AddSingleton<IConfiguration>(config);

            services.AddTransient<QuartzService>();
            services.AddScoped<IJobFactory, JobFactory>();
            services.AddSingleton(service =>
            {
                var jobFactory = service.GetService<IJobFactory>();
                ArgumentNullException.ThrowIfNull(jobFactory);
                scheduler.JobFactory = jobFactory;
                return scheduler;
            });

            services.AddScoped<TestJob>();

            ServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
