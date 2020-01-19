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
        [Authorize]
        public IActionResult SupportPage()
        {
            return View();
        }
    }
}
