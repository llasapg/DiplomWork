using System;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Linq;
using DiplomaSolution.ViewModels;
using System.Security.Claims;
using DiplomaSolution.Services.Interfaces;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : Controller
    {
        public SignInManager<ServiceUser> SignInManager { get; set; }
        public UserManager<ServiceUser> UserManager { get; set; }
        public ISendEmailService SendEmailService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public AccountController(SignInManager<ServiceUser> signInManager,
            UserManager<ServiceUser> userManager,
            ISendEmailService sendEmailService)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            SendEmailService = sendEmailService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync(); // checks what providers we have configured in class startup.cs

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            var userData = await UserManager.FindByEmailAsync(customer.EmailAddress);

            if (userData != null)
            {
                var providedData = await SignInManager.CheckPasswordSignInAsync(userData, customer.Password, false);

                if (providedData.ToString() == "Failed") // Provided wrong email or password --> we dont want to identify user that he guissed password
                {
                    ModelState.AddModelError("", "Error occured, login is not possible...");

                    return View(viewModel);
                }
                else if (providedData.Succeeded) // Check that customer has a verified email
                {
                    if (userData.EmailConfirmed == false)
                    {
                        ModelState.AddModelError("", "Email is not confirmed");

                        return View(viewModel);
                    }
                    else
                    {
                        var loginResponse = await SignInManager.PasswordSignInAsync(userData, customer.Password, false, false);

                        if (loginResponse.Succeeded && returnUrl != null)
                        {
                            return Redirect(returnUrl);
                        }
                        if(loginResponse.Succeeded)
                        {
                            return RedirectToAction("Index", "HomePage");
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Error occured, login is not possible...");

            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]// this is used only for displaing available provides
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync(); // checks what providers we have configured in class startup.cs

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [AllowAnonymous] //todo
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "HomePage");
            }

            var customer = await UserManager.FindByIdAsync(userId);

            if (customer != null)
            {
                var confirmationEmailResult = await UserManager.ConfirmEmailAsync(customer, token);

                if (confirmationEmailResult.Succeeded)
                {
                    await SignInManager.SignInAsync(customer, false);

                    return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    return RedirectToAction("Index", "HomePage");
                }
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogIn(string provider, string returnUrl) // тут мы уже просто берем переданный провайдер и выполняем редирект
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl });

            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [AllowAnonymous]//todo
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/"); // root directory of our application

            if (remoteError == null)
            {
                var info = await SignInManager.GetExternalLoginInfoAsync();

                if (info != null)
                {
                    // Search for the customer in our db

                    var result = await UserManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));

                    if (result == null) // new account
                    {
                        var serviceUser = new ServiceUser
                        {
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = info.Principal.FindFirstValue(ClaimTypes.GivenName)
                        };

                        var userResponse = await UserManager.CreateAsync(serviceUser);

                        await UserManager.AddClaimAsync(serviceUser, new Claim("UploadPhoto", "true"));

                        await UserManager.AddToRoleAsync(serviceUser, "User");

                        if (userResponse.Succeeded)
                        {
                            await UserManager.AddLoginAsync(serviceUser, info); // Creates point in AspNetUserLogins

                            var token = await UserManager.GenerateEmailConfirmationTokenAsync(serviceUser);

                            var emailUrlConfirmation = Url.Action("ConfirmEmail", "Account", new { UserId = serviceUser.Id, Token = token }, Request.Scheme);

                            return RedirectToAction("ConfirmPartnerRegister", "Registration", new { EmailUrlConfirmation = emailUrlConfirmation });
                        }
                    }
                    else
                    {
                        if (!result.EmailConfirmed)
                        {
                            var loginsData = await UserManager.GetLoginsAsync(result);

                            if (loginsData == null)
                            {
                                await UserManager.AddLoginAsync(result, info);
                            }
                            var token = await UserManager.GenerateEmailConfirmationTokenAsync(result);

                            var emailUrlConfirmation = Url.Action("ConfirmEmail", "Account", new { UserId = result.Id, Token = token }, Request.Scheme);

                            return RedirectToAction("ConfirmPartnerRegister", "Registration", new { EmailUrlConfirmation = emailUrlConfirmation });
                        }
                        else
                        {
                            var loginReponse = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

                            if (loginReponse.Succeeded)
                            {
                                return RedirectToAction("Index", "HomePage");
                            }
                            else
                            {
                                await UserManager.AddLoginAsync(result, info);

                                await SignInManager.SignInAsync(result, false);

                                return RedirectToAction("Index", "HomePage");
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Login failed, please contact our support team");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login failed, partner issue");
            }

            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("Index", "HomePage");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordWithEmail()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordConfirmationModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    var response = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (response.Succeeded)
                    {
                        return RedirectToAction("Index", "HomePage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Token is not valid");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User is not found");
                }
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                ModelState.AddModelError("", "Wrong token or user");
            }

            return View(new ResetPasswordConfirmationModel { UserId = userId, Token = token });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordConfirmationPage(ResetPasswordModel model)
        {
            // Add logic to send confirmation email with token

            if (model != null) // check that email is confirmed
            {
                var user = await UserManager.FindByEmailAsync(model.EmailAddress);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                        var url = Url.Action("ResetPassword", "Account", new { UserId = user.Id, Token = token }, Request.Scheme);

                        var result = await SendEmailService.SendEmail(new ServiceEmail
                        {
                            EmailHtmlText = $"<strong>Hello, {user.UserName}, here is your password reset link - {url}</strong>",
                            EmailSubject = "Password reset",
                            EmailText = $"Hello, {user.UserName}, here is your password reset link - {url}",
                            FromEmail = "testEmailAddress@gmail.com",
                            FromName = "Yevhen",
                            ToEmail = user.Email,
                            ToName = user.UserName
                        });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Plese confirm your email before this operation");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Some error occured, please contact local admin");
                }
            }
            else
            {
                ModelState.AddModelError("", "Some error occured, please contact local admin");
            }

            return View("ResetPasswordConfirmationPage", model.EmailAddress);
        }
    }
}
