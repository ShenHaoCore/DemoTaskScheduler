using Hangfire;
using System;
using System.Web.Http;

namespace TaskService3.Controllers
{
    /// <summary>
    /// 作业
    /// </summary>
    public class JobController : ApiController
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Create()
        {
            RecurringJob.AddOrUpdate<RecurringJobService>("SimpleJob", X => X.SimpleJob(null), Cron.Minutely(), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local });
            return "OK";
        }
    }
}
