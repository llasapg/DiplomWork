using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using DiplomaSolution.Helpers.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// Main page controller
    /// </summary>
    [Authorize(Policy = "DefaultMainPolicy")]
    public class HomePageController : Controller
    {
        /// <summary>
        /// File manager service ( upload, etc.. )
        /// </summary>
        private IFileManagerService FileManagerService { get; set; }
        /// <summary>
        /// Simple factory to create the logger
        /// </summary>
        private ILoggerFactory ILoggerFactory { get; set; }
        /// <summary>
        /// Logger class to perform logging to the file
        /// </summary>
        private ILogger<FileLogger> Logger { get; set; }
        /// <summary>
        /// Service to perform customer data manipulation
        /// </summary>
        private UserManager<ServiceUser> UserManager { get; set; }

        /// <summary>
        /// Service resolving
        /// </summary>
        /// <param name="service"></param>
        public HomePageController(IFileManagerService service, ILoggerFactory loggerFactory, UserManager<ServiceUser> userManager)
        {
            ILoggerFactory = loggerFactory;
            FileManagerService = service;
            Logger = ILoggerFactory.CreateLogger<FileLogger>();
            UserManager = userManager;
        }

        /// <summary>
        /// Action to return main page of the application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            Logger.LogCritical("Index action hitted ( first log) ");

            return View();
        }

        /// <summary>
        /// Action to upload the photo to web-site
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(IndexViewData data)
        {
            if (data.FormFileData != null)
            {
                var customerData = await UserManager.GetUserAsync(User);

                if(customerData != null)
                {
                    //todo - think about how we can handle errors in case of bad data and etc...
                    await FileManagerService.LoadFileToTheDB(data.FormFileData, customerData.Id);
                }
            }

            var viewModel = new IndexViewData { FormFileData = data.FormFileData };

            return View(viewModel);
        }
    }
}
