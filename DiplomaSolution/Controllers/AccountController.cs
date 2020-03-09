using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DiplomaSolution.ViewModels;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using DiplomaSolution.Helpers.ErrorResponseMessages;
using System.Diagnostics;
using System.Linq;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// Controller to perform actions with accounts ( Create / ChangePassword and etc... )
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Basic construstor to perform DI
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public AccountController(SignInManager<ServiceUser> signInManager, UserManager<ServiceUser> userManager, ISendEmailService sendEmailService, IDataProtectionProvider dataProtecttionProvider, IAccountService accountService)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            SendEmailService = sendEmailService;
            DataProtectionProvider = dataProtecttionProvider;
            Protector = DataProtectionProvider.CreateProtector("DataProtection");
            AccountService = accountService;
        }

        #region DI services

        private SignInManager<ServiceUser> SignInManager { get; set; }
        private UserManager<ServiceUser> UserManager { get; set; }
        private ISendEmailService SendEmailService { get; set; }
        private IDataProtectionProvider DataProtectionProvider { get; set; }
        private IDataProtector Protector { get; set; }
        private IAccountService AccountService { get; set; }

        #endregion

        #region Login actions ( Login / Logout / External Login )

        /// <summary>
        /// Basic action to perform login for both external and our own customers
        /// </summary>
        /// <param name="customer">Customer model</param>
        /// <param name="returnUrl">Optional - can perofrm redirect later to this url</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync();

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            var loginResponse = await AccountService.LoginCustomer(customer, returnUrl);

            if(loginResponse.StatusCode == 404) // Add email and password removing in case if its wrong
            {
                foreach (var item in loginResponse.ValidationErrors)
                {
                    ModelState.AddModelError("", item);
                }
            }
            else if(loginResponse.StatusCode == 300)
            {
                return Redirect(returnUrl);
            }
            else // 200 status code --> redirect to Home page with cookies created and stored in browser
            {
                return Redirect(Url.Action("Index", "HomePage"));
            }

            return View(viewModel);
        }

        /// <summary>
        /// Get action to return login page ( currently is not fully inmplemented )
        /// </summary>
        /// <param name="returnUrl">Optional - can perofrm redirect later to this url</param>
        /// <returns>Login page</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync();

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            return View(viewModel);
        }

        /// <summary>
        /// Action to perform extermal provider login ( Google, Facebook and etc... )
        /// </summary>
        /// <param name="provider">Provider name</param>
        /// <param name="returnUrl">Optional - can perofrm redirect later to this url</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogIn(string provider, string returnUrl = null) 
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl });

            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl); // Check it

            return new ChallengeResult(provider, properties); // OAuth using selected provider
        }

        /// <summary>
        /// External login callback to perform login operations on our side if login was successful
        /// </summary>
        /// <param name="returnUrl">Optional - can perofrm redirect later to this url</param>
        /// <param name="remoteError">Optional - can be null, if everything is okey while login on provider side</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            if (remoteError == null)
            {
                var loginResponse = await AccountService.ExternalLoginCallBack(returnUrl, remoteError);

                if(loginResponse.StatusCode == 300 && loginResponse.ValidationErrors.Count == 0)
                {
                    return Redirect(loginResponse.RedirectUrl);
                }
                else if(loginResponse.StatusCode == 200 && loginResponse.ValidationErrors.Count == 0)
                {
                    return Redirect(loginResponse.RedirectUrl);
                }
                else if(loginResponse.StatusCode == 404 && loginResponse.ValidationErrors.Count > 0)
                {
                    foreach (var item in loginResponse.ValidationErrors) // load all provided errors
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }

            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync();

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            return View("Login", viewModel);
        }

        /// <summary>
        /// Action to perdorm user logout ( delete cookies and clear User prop of basic controller class )
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("Index", "HomePage");
        }

        #endregion

        #region Email confirmation actions

        /// <summary>
        /// Action to perform email confirmation --> filed EmailConfirmed is changing to true
        /// </summary>
        /// <param name="userId">User id, to find it</param>
        /// <param name="token">Generated token</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return Redirect(Url.Action("Index", "HomePage")); // no actions
            }

            var customer = await UserManager.FindByIdAsync(userId);

            if (customer != null)
            {
                var confirmationEmailResult = await UserManager.ConfirmEmailAsync(customer, token);

                if (confirmationEmailResult.Succeeded)
                {
                    await SignInManager.SignInAsync(customer, false);

                    return Redirect(Url.Action("Index", "HomePage")); // Login and redirect to index page
                }
                else
                {
                    return Redirect(Url.Action("Index", "HomePage")); // no actions
                }
            }
            else
            {
                return Redirect(Url.Action("Index", "HomePage")); // no actions
            }
        }

        #endregion

        #region Access denied actions

        /// <summary>
        /// Access denied page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        #endregion

        #region Password reset actions

        /// <summary>
        /// Action to return reset password view ( First step )
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordWithEmail()
        {
            return View();
        }

        /// <summary>
        /// // todo ( add check, that model is correct ) 
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
        /// Step 3
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

        /// <summary>
        /// (Step 2)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordConfirmationPage(ResetPasswordModel model)
        {
            if (model != null)
            {
                var user = await UserManager.FindByEmailAsync(model.EmailAddress);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                        var url = Url.Action("ResetPassword", "Account", new { UserId = user.Id, Token = token }, Request.Scheme);

                        await SendEmailService.SendEmail(new ServiceEmail
                        {
                            EmailHtmlText = $"<strong>Hello, {user.UserName}, here is your password reset link - {url}</strong>",
                            EmailSubject = "Password reset",
                            EmailText = $"Hello, {user.UserName}, here is your password reset link - {url}",
                            FromEmail = "testEmailAddress@gmail.com",
                            FromName = "Yevhen",
                            ToEmail = user.Email,
                            ToName = user.UserName
                        });

                        Trace.WriteLine($"Email was send to customer {user.Id} - id");
                    }
                    else
                    {
                        ModelState.AddModelError("", DefaultResponseMessages.EmailIsNotVerified);

                        Trace.WriteLine($"Email was not verified for customer {user.Id} - id");
                    }
                }
                else
                {
                    ModelState.AddModelError("", DefaultResponseMessages.CustomerIsNotFoundInDb);

                    Trace.WriteLine($"Wrong login attempt for email {model.EmailAddress}");
                }

                return View("ResetPasswordConfirmationPage", model.EmailAddress);
            }
            else
            {
                ModelState.AddModelError("", DefaultResponseMessages.EmailIsNotProvided);
            }

            return View();
        }

        #endregion

        #region Add password actions

        

        #endregion
    }
}
