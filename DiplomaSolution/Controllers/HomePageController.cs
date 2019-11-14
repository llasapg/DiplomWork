using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace DiplomaSolution.Controllers
{
    public class HomePageController : Controller
    {
        public HomePageController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
