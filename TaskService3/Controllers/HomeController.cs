using System.Web.Http;

namespace TaskService3.Controllers
{
    /// <summary>
    /// Home 控制器
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return new { code = 1, msg = "OK HomeController" };
        }
    }
}
