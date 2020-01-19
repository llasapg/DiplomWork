using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaSolution.Controllers
{
    public class HomePageController : Controller
    {
        private IFileManagerService FileManagerService { get; set; }

        public HomePageController(IFileManagerService fileManagerService)
        {
            FileManagerService = fileManagerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(IndexViewData data) // Переделано с использованием сессий
        {
            Customer currentUser = null;

            if (data.FormFileData != null)
            {
                FileManagerService.LoadFileToTheServer(data.FormFileData);
            }

            var viewModel = new IndexViewData { Customer = currentUser, FormFileData = data.FormFileData };

            return View(viewModel);
        }
    }
}
