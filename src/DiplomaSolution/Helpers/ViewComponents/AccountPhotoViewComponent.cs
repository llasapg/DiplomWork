using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Helpers.ViewComponents
{
    /// <summary>
    /// Component to display all customer edited photos
    /// </summary>
    [ViewComponent(Name = "AccountPhoto")]
    public class AccountPhotoViewComponent : ViewComponent
    {
        private UserManager<ServiceUser> UserManager { get; set; }
        private CustomerContext DataContext { get; set; }

        public AccountPhotoViewComponent(UserManager<ServiceUser> userManager, CustomerContext customerContext) // getting all needed services
        {
            UserManager = userManager;
            DataContext = customerContext;
        }

        /// <summary>
        /// Main method in this view component to display customer ( current logined ) photos
        /// </summary>
        /// <param name="imageNumber"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int imageNumber) // this method should be defined in case if u want to use this view component
        {
            var currentUser = await UserManager.FindByNameAsync(User.Identity.Name);

            var uploadedPhotos = DataContext.CustomerImageFiles.Where(item => item.CustomerId == currentUser.Id).Select(item => item).ToList();

            var editedPhotos = DataContext.CustomerEditedImageFiles.Where(item => item.CustomerId == currentUser.Id).Select(item => item).ToList();

            var viewModel = new AccountEditedImagesViewModel { EditedImages = new List<string>(), OriginalImages = new List<string>() };

            foreach (var item in editedPhotos)
            {
                viewModel.EditedImages.Add("../" + Path.Combine("CustomersImages", Path.GetFileName(item.FullName)));
            }

            foreach (var item in uploadedPhotos)
            {
                viewModel.OriginalImages.Add("../" + Path.Combine("CustomersImages", Path.GetFileName(item.FullName)));
            }

            return View("AccountPhoto", viewModel);
        }
    }
}
