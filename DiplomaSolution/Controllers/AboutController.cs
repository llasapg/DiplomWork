using System;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class AboutController : Controller
    {
        public AboutController()
        {
        }

        [HttpGet]
        //[ActionName("GetInfo")]
        public IActionResult GiveInfo()
        {
            return View();
        }
    }
}
