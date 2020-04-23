using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    [Authorize(Policy = "DefaultMainPolicy")]
    public class InfoPageController : Controller
    {
        public InfoPageController()
        {
        }

        [HttpGet]        
        public IActionResult SupportPage()
        {
            return View();
        }
    }
}
