using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using DiplomaSolution.Helpers.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using ImageMagick;
using System.Diagnostics;
using System.IO;

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
        /// Prop to work with the DB
        /// </summary>
        private CustomerContext CustomerContext { get; set; }

        /// <summary>
        /// Service resolving
        /// </summary>
        /// <param name="service"></param>
        public HomePageController(IFileManagerService service, ILoggerFactory loggerFactory, UserManager<ServiceUser> userManager, CustomerContext customerContext)
        {
            ILoggerFactory = loggerFactory;
            FileManagerService = service;
            Logger = ILoggerFactory.CreateLogger<FileLogger>();
            UserManager = userManager;
            CustomerContext = customerContext;
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

            if (data.FormFileData != null)
            {
                var customerData = await UserManager.GetUserAsync(User);
                
                if(customerData != null)
                {
                    //todo - think about how we can handle errors in case of bad data and etc...
                    fileUploadResponse = await FileManagerService.LoadFileToTheServer(data.FormFileData, customerData.Id);
                }
            }

            if(fileUploadResponse.ValidationErrors != null)
            {
                foreach (var error in fileUploadResponse.ValidationErrors) // using this stuff we are adding errors to the validation summary element ( todo - add it !!!! ASAP )
                {
                    ModelState.AddModelError("", error);
                }
            }

            var viewModel = new IndexViewData();

            viewModel.PathToTheInputImage = Path.Combine("CustomersImages", Path.GetFileName(fileUploadResponse.ResponseData.ToString()));

            return View(viewModel);
        }

        /// <summary>
        /// Action to change the photo
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost] //todo - remove this logic to the fileService and add more options for file edit ( + add ability to reset changes and check, that customer didnt select any file )
        public async Task<IActionResult> ModifyPhoto(IndexViewData data)
        {
            // достать последнее изображение пользователя ( или же сохранить его в модели ) и обработать его

            var customerName = User.Identity.Name;

            var customerData = await UserManager.FindByNameAsync(customerName);

            var lastUploadedImageName = CustomerContext.CustomerImageFiles.ToList()
                .Where(imageFile => imageFile.CustomerId == customerData.Id)
                .OrderByDescending(orderBy => orderBy.UploadTime) // get the last user photo
                .Select(response => response.FullName);

            //todo - remove this to the service ( logic with files )

            // 1 Upload photo

            var editedFileName = "";

            using (var image = new MagickImage(lastUploadedImageName.First()))
            {
                image.Alpha(AlphaOption.Background);
                image.AutoGamma(Channels.Blue);
                image.Border(10, 10);
                editedFileName = lastUploadedImageName.First().Replace(Path.GetFileNameWithoutExtension(lastUploadedImageName.First()), Path.GetFileNameWithoutExtension(lastUploadedImageName.First()) + "_modified");
                using (System.IO.File.Create(editedFileName))
                image.Write(editedFileName);
            }

            var viewModel = new IndexViewData();

            viewModel.PathToTheInputImage = "../" + data.PathToTheInputImage;
            viewModel.PathToTheResultImage = "../" + Path.Combine("CustomersImages", Path.GetFileName(editedFileName));

            return View("Index", viewModel);
        }
    }
}
