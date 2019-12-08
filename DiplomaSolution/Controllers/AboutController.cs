using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult GiveInfo()
        {
            return View();
        }
    }
}
