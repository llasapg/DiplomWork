using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class HomePageController : Controller
    {
        private IFileManagerService FileManagerService { get; set; }
        private ILogInService LogInService { get; set; }

        public HomePageController(IFileManagerService fileManagerService, ILogInService logInService)
        {
            FileManagerService = fileManagerService;
            LogInService = logInService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexViewData data) // Переделано с использованием сессий
        {
            Customer currentUser = null;

            if (data.FormFileData != null)
            {
                FileManagerService.LoadFileToTheServer(data.FormFileData);
            }

            if (data.Customer != null)
            {
                currentUser = LogInService.LogIn(data.Customer.EmailAddress);
            }

            var viewModel = new IndexViewData { Customer = currentUser, FormFileData = data.FormFileData };

            return View(viewModel);
        }
    }
}
