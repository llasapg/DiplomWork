using System;
using System.ComponentModel;
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
        [Authorize(Policy = "DefaultMainPolicy")]
        public IActionResult SupportPage()
        {
            var currentUser = HttpContext.User.Identity.Name;

            return View(currentUser);
        }
    }
}
