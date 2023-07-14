using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.BasicAuthorization;
using TaskService2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
    config.UseConsole();
    config.UseDefaultActivator();
});
builder.Services.AddHangfireServer();

var app = builder.Build();

var option = new BasicAuthAuthorizationFilterOptions { RequireSsl = false, SslRedirect = false, LoginCaseSensitive = true, Users = new[] { new BasicAuthAuthorizationUser { Login = "admin", PasswordClear = "123" } } };
app.UseHangfireDashboard("/hangfire", new DashboardOptions() { DashboardTitle = "任务调度中心", Authorization = new List<BasicAuthAuthorizationFilter> { new BasicAuthAuthorizationFilter(option) } });
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync("<a href='/hangfire'>任务调度中心</a>");
    });
});

RecurringJob.AddOrUpdate<RecurringJobService>("SimpleJob", X => X.SimpleJob(null!), Cron.Minutely(), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local });

app.Run();
