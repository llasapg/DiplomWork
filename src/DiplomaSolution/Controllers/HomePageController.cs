using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IO;
using DiplomaSolution.Services.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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
        /// Service to perform customer data manipulation
        /// </summary>
        private UserManager<ServiceUser> UserManager { get; set; }
        /// <summary>
        /// Logger class
        /// </summary>
        private ILogger<HomePageController> Logger { get; set; }

        /// <summary>
        /// Service resolving
        /// </summary>
        /// <param name="service"></param>
        public HomePageController(IFileManagerService service, UserManager<ServiceUser> userManager, ILogger<HomePageController> logger)
        {
            FileManagerService = service;
            UserManager = userManager;
            Logger = logger;
        }

        /// <summary>
        /// Action to return modify image view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditImage()
        {
            var homePageModel = new IndexViewData();

            return View(homePageModel);
        }

        /// <summary>
        /// Action to return main page of the application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action to upload the photo to web-site
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditImage(IndexViewData data)
        {           
            var fileUploadResponse = new DefaultServiceResponse();

            var viewModel = new IndexViewData();

            if (data.FormFileData != null)
            {
                var customerData = await UserManager.GetUserAsync(User);
                
                if(customerData != null)
                {
                    fileUploadResponse = await FileManagerService.LoadFileToTheServer(data.FormFileData, customerData.Id);
                }

                if (fileUploadResponse.ValidationErrors != null)
                {
                    foreach (var error in fileUploadResponse.ValidationErrors) 
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                viewModel.PathToTheInputImage = Path.Combine("../","CustomersImages",
                    Path.GetFileName(fileUploadResponse.ResponseData.ToString()));

                Logger.LogInformation($"Path to the input image, uploaded by the customer - {viewModel.PathToTheInputImage}");
            }
            else
            {
                ModelState.AddModelError("", "In order to edit photo, you should first provide it");
            }

            return View(viewModel);
        }

        /// <summary>
        /// Action to change the photo
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ModifyPhoto(IndexViewData data)
        {
            if(!string.IsNullOrEmpty(data.PathToTheInputImage))
            {
                var customerData = await UserManager.GetUserAsync(User);

                var fileResponse = await FileManagerService.ModifyFile(new ModifyModel {
                    UserId = customerData.Id,
                    OutputFileType = data.SelectedResponseFileFormat,
                    SelectedOperation = data.SelectedFileOperation,
                    Intesivity = data.Intensity,
                    UseFrame = data.UseFrame });

                Logger.LogInformation($"Path to the result image, uploaded by the customer - {fileResponse.ResponseData.ToString()}");

                return View("EditImage", new IndexViewData() { PathToTheInputImage = "../" + data.PathToTheInputImage,
                    PathToTheResultImage = fileResponse.ResponseData.ToString() });
            }
            else
            {
                ModelState.AddModelError("", "In order to edit photo, you should first provide it");

                return View("EditImage", new IndexViewData());
            }
        }

        /// <summary>
        /// Method to download files from the server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Download(IndexViewData data)
        {
            var fileName = data.PathToTheResultImage.ToString();

            var filepath = "/app/wwwroot" + Path.GetFullPath(fileName);

            Logger.LogInformation($"Requested file name for the download - {filepath}");

            var memory = new MemoryStream();

            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(filepath), Path.GetFileName(filepath));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes() => new Dictionary<string, string>{{".png", "image/png"},{".jpg", "image/jpeg"}};
    }
}
