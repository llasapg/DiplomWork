using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class InfoPageController : Controller
    {
        public InfoPageController()
        {
        }

        [HttpGet]
        //[Authorize(Policy = "DefaultMainPolicy")]
        public IActionResult SupportPage()
        {
            var currentUser = HttpContext.User.Identity.Name;

            var feature1 = HttpContext.Features.Get<IHttpRequestFeature>();

            var feature2 = HttpContext.Features.Get<IHttpResponseFeature>();

            return StatusCode(200, new { }); // As we can see ( this is 

            //return View(currentUser);
        }
    }
}
