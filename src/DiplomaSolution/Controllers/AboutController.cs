using DiplomaSolution.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// Controller to get info about web-site
    /// </summary>
    public class AboutController : Controller
    {
        private ILoggerFactory LoggerFactory { get; set; }
        private ILogger Logger { get; set; }
        public AboutController(ILoggerFactory factory)
        {
            LoggerFactory = factory;
            Logger = LoggerFactory.CreateLogger("superCool");
        }

        /// <summary>
        /// Method, that returns basic info pages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionFilter, ExceptionFilter, ResourseFilter]
        public IActionResult GiveInfo()
        {
            Logger.LogInformation("Test log {UserId}", 200);
            return View();
        }
    }
}
 