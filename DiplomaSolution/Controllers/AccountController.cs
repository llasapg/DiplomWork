using System;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Linq;
using DiplomaSolution.ViewModels;

namespace DiplomaSolution.Controllers
{
    public class AccountController : Controller
    {
        public SignInManager<ServiceUser> SignInManager { get; set; }
        public UserManager<ServiceUser> UserManager { get; set; }

        public AccountController(SignInManager<ServiceUser> signInManager,
            UserManager<ServiceUser> userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            var registerReponse = await UserManager.FindByEmailAsync(customer.EmailAddress);

            if(registerReponse != null)
            {
                var result = await SignInManager.PasswordSignInAsync(registerReponse.UserName, customer.Password, false, false);

                if (result.Succeeded && returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "HomePage");
            }
            else
            {
                ModelState.AddModelError("", "Error occured, login is not possible...");

                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync();

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogIn(string provider, string returnUrl) // тут мы уже просто берем переданный провайдер и выполняем редирект
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl});

            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("Index", "HomePage");
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }
    }
}
