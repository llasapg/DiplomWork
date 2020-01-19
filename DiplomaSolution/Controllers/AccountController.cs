using System;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaSolution.Controllers
{
    public class AccountController : Controller
    {
        public SignInManager<IdentityUser> SignInManager { get; set; }

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(IndexViewData user, string returnUrl)
        {
            var result = await SignInManager.PasswordSignInAsync(user.Customer.FirstName, user.Customer.Password, false, false);

            if(result.Succeeded)
            {
               Redirect(returnUrl);
            }

            ModelState.AddModelError("", "Error occured, login is not possible...");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("Index", "HomePage");
        }
    }
}
