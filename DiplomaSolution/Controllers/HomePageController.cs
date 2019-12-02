using System;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class HomePageController : Controller
    {
        private IFileManagerService FileManagerService { get; set; }

        public HomePageController (IFileManagerService fileManagerService)
        {
            FileManagerService = fileManagerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            FileManagerService.LoadFileToTheServer(file);

            var viewModel = new FormFile { FullName = "/CustomersImages/" + file.FileName };

            return View(viewModel);
        }
    }
}
