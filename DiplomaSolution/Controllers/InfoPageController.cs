using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class InfoPageController : Controller
    {
        public InfoPageController()
        {
        }

        [HttpGet]
        [Authorize(Policy = "ShitPolicy")]
        public IActionResult SupportPage()
        {
            return View();
        }
    }
}
