﻿using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DiplomaSolution.Services.Interfaces;
using System.Diagnostics;
using DiplomaSolution.Helpers.ErrorResponseMessages;

//todo - Remove logic to the needed services
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
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmationPage(Customer customer)
        {
            if (ModelState.IsValid)
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
                    ModelState.AddModelError("", DefaultResponseMessages.AllreadyHasAccount);

                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Action to confirm partner registration
        /// </summary>
        /// <param name="emailUrlConfirmation"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ConfirmPartnerRegister(string emailUrlConfirmation)
        {
            return View("ConfirmationPage", emailUrlConfirmation);
        }

        /// <summary>
        /// Action to return registration form
        /// </summary>
        /// <returns></returns>
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
