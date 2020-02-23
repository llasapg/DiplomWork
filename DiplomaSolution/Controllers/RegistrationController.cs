using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DiplomaSolution.Services.Interfaces;
using System.Diagnostics;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// This controller is used to create customer and etc stuff...
    /// </summary>
    public class RegistrationController : Controller
    {
        /// <summary>
        /// Ctor to get all needed DI services
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="sendEmailService"></param>
        public RegistrationController(ILogger<RegistrationController> logger, UserManager<ServiceUser> userManager, SignInManager<ServiceUser> signInManager, RoleManager<IdentityRole> roleManager, ISendEmailService sendEmailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Logger = logger;
            SendEmailService = sendEmailService;
        }

        #region DI services

        private ILogger<RegistrationController> Logger { get; set; }
        private UserManager<ServiceUser> UserManager { get; set; }
        private SignInManager<ServiceUser> SignInManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }
        private ISendEmailService SendEmailService { get; set; }

        #endregion

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
            var userSearchResult = await UserManager.FindByEmailAsync(customer.EmailAddress);

            Trace.WriteLine($"User search response - {userSearchResult}");

            if (userSearchResult == null)
            {
                var user = new ServiceUser
                {
                    Email = customer.EmailAddress,
                    UserName = customer.FirstName
                };

                var result = await UserManager.CreateAsync(user, customer.Password);

                Trace.WriteLine($"User creation response - {result}");

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

                    var response = await SendEmailService.SendEmail(new ServiceEmail
                    {
                        FromEmail = "testEmailAddress@gmail.com",
                        FromName = "Yevhen",
                        ToEmail = currentUser.Email,
                        ToName = currentUser.UserName,
                        EmailSubject = "Thank you for register!!!",
                        EmailHtmlText = $"<strong>Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}</strong>",
                        EmailText = $"Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}",
                    });

                    Trace.WriteLine($"SendGrid email send response - {response.Body}");

                    return View("ConfirmationPage", currentUser.UserName);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    Logger.LogError("Model is not valid or email is currently registered!");

                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Hey, you already have account, try to login please");

                return View();
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

        #region Helpers

        /// <summary>
        /// Method to verify, that we have needed role, before appling it to customer
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
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

        #endregion
    }
}
