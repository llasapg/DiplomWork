using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using DiplomaSolution.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Services.Classes
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Main service from Identity to perform signIn of the user
        /// </summary>
        private SignInManager<ServiceUser> SignInManager { get; set; }
        /// <summary>
        /// Main service from Identity to perform user register / remove / verification and etc...
        /// </summary>
        private UserManager<ServiceUser> UserManager { get; set; }
        /// <summary>
        /// Custom service to perform email sending using SendGrind functional
        /// </summary>
        private ISendEmailService SendEmailService { get; set; }
        /// <summary>
        /// System interface for security purpose
        /// </summary>
        private IDataProtectionProvider DataProtectionProvider { get; set; }
        /// <summary>
        /// Data protector to hide request data provided in query string
        /// </summary>
        private IDataProtector Protector { get; set; } 

        /// <summary>
        /// Basic construstor to perform DI
        /// </summary>
        public AccountService(SignInManager<ServiceUser> signInManager, UserManager<ServiceUser> userManager, ISendEmailService sendEmailService, IDataProtectionProvider dataProtecttionProvider)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            SendEmailService = sendEmailService;
            DataProtectionProvider = dataProtecttionProvider;
            Protector = DataProtectionProvider.CreateProtector("DataProtection");
        }

        /// <summary>
        /// Main method to perform login of the customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="returnUrl"></param>
        public async Task<IActionResult> LoginCustomer(LoginViewModel customer, string returnUrl = null)
        {
            var providers = await SignInManager.GetExternalAuthenticationSchemesAsync();

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, ListOfProviders = providers };

            var userData = await UserManager.FindByEmailAsync(customer.EmailAddress);

            if (userData != null)
            {
                var providedData = await SignInManager.CheckPasswordSignInAsync(userData, customer.Password, true); //todo - remove validation to account controller and perform other tasks there

                if (providedData.ToString() == "Failed") // If Failed --> wrong password and login combination
                {
                    ModelState.AddModelError("", DefaultResponseMessages.WrongPasswordAndEmailCombination);

                    return View(viewModel);
                }
                else if (providedData.IsLockedOut)
                {
                    ModelState.AddModelError("", DefaultResponseMessages.AccountIsLockOut);

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
                        else if (loginResponse.Succeeded)
                        {
                            return RedirectToAction("Index", "HomePage");
                        }
                    }
                }
            }

            ModelState.AddModelError("", DefaultResponseMessages.CustomerIsNotFoundInDb);

            return View(viewModel);

        }
    }
}
