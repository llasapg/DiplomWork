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
        /// <summary>
        /// Method, that returns basic info pages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionFilter, ExceptionFilter, ResourseFilter]
        public IActionResult GiveInfo()
        {
            return View();
        }
    }
}
 