using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GiveInfo()
        {
            return View();
        }
    }
}
