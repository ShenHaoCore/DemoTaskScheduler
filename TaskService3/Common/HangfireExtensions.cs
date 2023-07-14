using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Hangfire;
using Owin;
using System;
using Topshelf;
using Topshelf.HostConfigurators;
using Hangfire.Common;

namespace TaskService3
{
    /// <summary>
    /// Hangfire扩展
    /// </summary>
    public static class HangfireExtensions
    {
        /// <summary>
        /// 使用OWIN
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public static HostConfigurator UseOwin(this HostConfigurator configurator, string baseAddress)
        {
            if (string.IsNullOrWhiteSpace(baseAddress)) { throw new ArgumentNullException(nameof(baseAddress)); }
            configurator.Service(() => new Bootstrap { Address = baseAddress });
            return configurator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStorage"></typeparam>
        /// <param name="app"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static IGlobalConfiguration<TStorage> UseStorage<TStorage>(this IAppBuilder app, TStorage storage) where TStorage : JobStorage
        {
            if (storage == null) { throw new ArgumentNullException(nameof(storage)); }
            return GlobalConfiguration.Configuration.UseStorage(storage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IAppBuilder UseHangfireFilters(this IAppBuilder app, params JobFilterAttribute[] filters)
        {
            if (filters == null) throw new ArgumentNullException(nameof(filters));
            foreach (JobFilterAttribute filter in filters) { GlobalConfiguration.Configuration.UseFilter(filter); }
            return app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IAppBuilder UseDashboardMetric(this IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.ServerCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(SqlServerStorage.ActiveConnections);
            GlobalConfiguration.Configuration.UseDashboardMetric(SqlServerStorage.TotalConnections);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.RecurringJobCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.RetriesCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.AwaitingCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.ScheduledCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.ProcessingCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.SucceededCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.FailedCount);
            GlobalConfiguration.Configuration.UseDashboardMetric(DashboardMetrics.DeletedCount);
            return app;
        }
    }
}
