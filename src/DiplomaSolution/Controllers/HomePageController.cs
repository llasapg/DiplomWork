using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IO;
using DiplomaSolution.Services.Models;

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
        /// Service resolving
        /// </summary>
        /// <param name="service"></param>
        public HomePageController(IFileManagerService service, UserManager<ServiceUser> userManager)
        {
            FileManagerService = service;
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
            var homePageModel = new IndexViewData();

            return View(homePageModel);
        }

        /// <summary>
        /// Action to upload the photo to web-site
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(IndexViewData data)
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
                    foreach (var error in fileUploadResponse.ValidationErrors) // using this stuff we are adding errors to the validation summary element ( todo - add it !!!! ASAP )
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                viewModel.PathToTheInputImage = Path.Combine("CustomersImages", Path.GetFileName(fileUploadResponse.ResponseData.ToString()));
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
        public async Task<IActionResult> ModifyPhoto(IndexViewData data) // todo - add ability to customize the input ( like se
        {
            if(!string.IsNullOrEmpty(data.PathToTheInputImage))
            {
                var customerData = await UserManager.GetUserAsync(User);

                var fileResponse = await FileManagerService.ModifyFile(new ModifyModel { UserId = customerData.Id, OutputFileType = data.SelectedResponseFileFormat, SelectedOperation = data.SelectedFileOperation, Intesivity = data.Intensity, UseFrame = data.UseFrame }); // todo - add data from slider

                return View("Index", new IndexViewData() { PathToTheInputImage = "../" + data.PathToTheInputImage, PathToTheResultImage = fileResponse.ResponseData.ToString() });
            }
            else
            {
                ModelState.AddModelError("", "In order to edit photo, you should first provide it");

                return View("Index", new IndexViewData());
            }
        }
    }
}
