using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiplomaSolution.Controllers
{
    public class RegistrationController : Controller
    {
        public ILogger<RegistrationController> Logger { get; set; }
        public UserManager<IdentityUser> UserManager { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; set; }

        public RegistrationController(
            ILogger<RegistrationController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmationPage(Customer customer)
        {
            var user = new IdentityUser
            {
                Email = customer.EmailAddress,
                UserName = customer.FirstName
            };

            var result = await UserManager.CreateAsync(user, customer.Password);

            if (result.Succeeded) // errors will be displayed on validation-summary
            {
                await SignInManager.SignInAsync(user, false);

                return View();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                Logger.LogError("Model is not valid or email is currently registered!");

                return View("RegistrationForm");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegistrationForm()
        {
            return View();
        }
    }
}
