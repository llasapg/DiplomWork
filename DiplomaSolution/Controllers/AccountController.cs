using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DiplomaSolution.ViewModels;
using System.Security.Claims;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using DiplomaSolution.Helpers.ErrorResponseMessages;
using System.Diagnostics;

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
        public AccountController(SignInManager<ServiceUser> signInManager, UserManager<ServiceUser> userManager, ISendEmailService sendEmailService, IDataProtectionProvider dataProtecttionProvider)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            SendEmailService = sendEmailService;
            DataProtectionProvider = dataProtecttionProvider;
            Protector = DataProtectionProvider.CreateProtector("DataProtection");
        }

        #region DI services

        private SignInManager<ServiceUser> SignInManager { get; set; }
        private UserManager<ServiceUser> UserManager { get; set; }
        private ISendEmailService SendEmailService { get; set; }
        private IDataProtectionProvider DataProtectionProvider { get; set; }
        private IDataProtector Protector { get; set; }

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

            var userData = await UserManager.FindByEmailAsync(customer.EmailAddress);

            if (userData != null)
            {
                var providedData = await SignInManager.CheckPasswordSignInAsync(userData, customer.Password, true);

                if (providedData.ToString() == "Failed") // If Failed --> wrong password and login combination
                {
                    ModelState.AddModelError("", DefaultResponseMessages.WrongPasswordAndEmailCombination);

                    return View(viewModel);
                }
                else if (providedData.Succeeded) // Check that customer has a verified email
                {
                    if (userData.EmailConfirmed == false)
                    {
                        ModelState.AddModelError("", DefaultResponseMessages.EmailIsNotVerified);

                        return View(viewModel);
                    }
                    else
                    {
                        var loginResponse = await SignInManager.PasswordSignInAsync(userData, customer.Password, false, true);

                        if (loginResponse.Succeeded && returnUrl != null)
                        {
                            return Redirect(returnUrl);
                        }
                        else if(loginResponse.Succeeded)
                        {
                            return RedirectToAction("Index", "HomePage");
                        }
                    }
                }
            }

            ModelState.AddModelError("", DefaultResponseMessages.CustomerIsNotFoundInDb);

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
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            if (remoteError == null)
            {
                var accountDetailsFromProvider = await SignInManager.GetExternalLoginInfoAsync();

                if (accountDetailsFromProvider != null)
                {
                    // Search for the customer in our db

                    var result = await UserManager.FindByEmailAsync(accountDetailsFromProvider.Principal.FindFirstValue(ClaimTypes.Email));

                    if (result == null) // new account
                    {
                        var serviceUser = new ServiceUser
                        {
                            Email = accountDetailsFromProvider.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = accountDetailsFromProvider.Principal.FindFirstValue(ClaimTypes.GivenName)
                        };

                        var userResponse = await UserManager.CreateAsync(serviceUser);

                        await UserManager.AddClaimAsync(serviceUser, new Claim("UploadPhoto", "true"));

                        await UserManager.AddToRoleAsync(serviceUser, "User");

                        if (userResponse.Succeeded)
                        {
                            await UserManager.AddLoginAsync(serviceUser, accountDetailsFromProvider); // Creates point in AspNetUserLogins

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
                                await UserManager.AddLoginAsync(result, accountDetailsFromProvider);
                            }
                            var token = await UserManager.GenerateEmailConfirmationTokenAsync(result);

                            var emailUrlConfirmation = Url.Action("ConfirmEmail", "Account", new { UserId = result.Id, Token = token }, Request.Scheme);

                            return RedirectToAction("ConfirmPartnerRegister", "Registration", new { EmailUrlConfirmation = emailUrlConfirmation });
                        }
                        else
                        {
                            var loginReponse = await SignInManager.ExternalLoginSignInAsync(accountDetailsFromProvider.LoginProvider, accountDetailsFromProvider.ProviderKey, false);

                            if (loginReponse.Succeeded)
                            {
                                return returnUrl == null ? Redirect(Url.Action("Index", "HomePage")) : Redirect(returnUrl);
                            }
                            else
                            {
                                await UserManager.AddLoginAsync(result, accountDetailsFromProvider); //no data in table aspnetuserlogins --> we should add it 

                                await SignInManager.SignInAsync(result, false);

                                return Redirect(Url.Action("Index", "HomePage"));
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", DefaultResponseMessages.ExternalLoginFailed);
                }
            }
            else
            {
                ModelState.AddModelError("", DefaultResponseMessages.ExternalLoginFailed);

                return Redirect(Url.Action("Login", "Account"));
            }

            return Redirect(Url.Action("Login", "Account"));
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
    }
}
