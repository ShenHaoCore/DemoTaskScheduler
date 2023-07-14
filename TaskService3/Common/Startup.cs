using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Swashbuckle.Application;
using System;
using System.Net.Http.Formatting;
using System.Web.Http;

[assembly: OwinStartup(typeof(TaskService3.Startup))]
namespace TaskService3
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            #region Page
            var fileSystem = new PhysicalFileSystem(@".\Home"); //静态网站根目录
            var options = new FileServerOptions { EnableDefaultFiles = true, FileSystem = fileSystem };
            options.StaticFileOptions.FileSystem = fileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "Index.html" }; //默认页面(填写与静态网站根目录的相对路径)
            app.UseFileServer(options);
            #endregion

            #region WebApi
            HttpConfiguration config = new HttpConfiguration();

            //启用标记路由
            config.MapHttpAttributeRoutes();

            //配置默认 Web API 路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            //配置Swagger
            config.EnableSwagger(C =>
            {
                C.SingleApiVersion("v1", "TaskServiceAPI");
                C.IncludeXmlComments($@"{typeof(Startup).Assembly.GetName().Name}.xml");
            }).EnableSwaggerUi(C =>
            {
                C.DocumentTitle("任务服务接口");
            });

            //将默认XML返回数据格式改为JSON
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));

            app.UseWebApi(config);
            #endregion

            #region Hangfire
            var queues = new[] { "default", "apis", "jobs" };
            app.UseStorage(new SqlServerStorage("Hangfire", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) })).UseConsole();
            app.UseHangfireFilters(new AutomaticRetryAttribute { Attempts = 0 });
            app.UseDashboardMetric();
            app.UseHangfireDashboard();
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ShutdownTimeout = TimeSpan.FromMinutes(30),
                Queues = queues,
                WorkerCount = Math.Max(Environment.ProcessorCount, 20)
            });
            #endregion
        }
    }
}
