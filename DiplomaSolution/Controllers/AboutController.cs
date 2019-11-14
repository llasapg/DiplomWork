using System;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    [Route("About")]
    public class AboutController : Controller
    {
        public AboutController()
        {

        }

        [HttpGet]
        [Route("GetInfo")]
        public IActionResult GiveInfo()
        {
            return View();
        }
    }
}
