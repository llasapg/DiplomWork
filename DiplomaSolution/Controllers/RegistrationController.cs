using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DiplomaSolution.Services.Interfaces;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// This controller is used to create customer and apply to them roles
    /// </summary>
    public class RegistrationController : Controller
    {
        public ILogger<RegistrationController> Logger { get; set; }
        public UserManager<ServiceUser> UserManager { get; set; }
        public SignInManager<ServiceUser> SignInManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public ISendEmailService SendEmailService { get; set; }

        public RegistrationController(ILogger<RegistrationController> logger,UserManager<ServiceUser> userManager,SignInManager<ServiceUser> signInManager,RoleManager<IdentityRole> roleManager, ISendEmailService sendEmailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Logger = logger;
            SendEmailService = sendEmailService;
        }

        /// <summary>
        /// Add there a return url to proceed with the customers ...
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [AllowAnonymous] //todo
        public async Task<IActionResult> ConfirmationPage(Customer customer)
        {
            var user = new ServiceUser
            {
                Email = customer.EmailAddress,
                UserName = customer.FirstName
            };

            var result = await UserManager.CreateAsync(user, customer.Password);

            if (result.Succeeded) // errors will be displayed on validation-summary
            {
                if (CheckIfWeHaveRole("User"))
                    await UserManager.AddToRoleAsync(user, "User");
                else
                {
                    await RoleManager.CreateAsync(new IdentityRole { Name = "User" });
                    await UserManager.AddToRoleAsync(user, "User");
                }

                var currentUser = await UserManager.FindByEmailAsync(user.Email);

                await UserManager.AddClaimAsync(currentUser, new Claim("UploadPhoto", "true"));

                var token = await UserManager.GenerateEmailConfirmationTokenAsync(currentUser);

                var emailUrlConfirmation = Url.Action("ConfirmEmail", "Account", new { UserId = currentUser.Id, Token = token }, Request.Scheme);

                var response = await SendEmailService.SendEmail(new ServiceEmail {
                    FromEmail = "testEmailAddress@gmail.com",
                    FromName = "Yevhen",
                    ToEmail = currentUser.Email,
                    ToName = currentUser.UserName,
                    EmailSubject = "Thank you for register!!!",
                    EmailHtmlText = $"<strong>Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}</strong>",
                    EmailText = $"Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}",
                });

                return View("ConfirmationPage");
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
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ConfirmPartnerRegister(string emailUrlConfirmation)
        {
            return View("ConfirmationPage", emailUrlConfirmation);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegistrationForm()
        {
            return View();
        }

        [NonAction]
        private bool CheckIfWeHaveRole(string roleName)
        {
            var defaultUserRole = new IdentityRole { Name = roleName };

            foreach (var role in RoleManager.Roles)
            {
                if (role.Name == defaultUserRole.Name) // that means that we have this role in DB
                {
                    return true;
                }
            }
            return false;
        }
    }
}
