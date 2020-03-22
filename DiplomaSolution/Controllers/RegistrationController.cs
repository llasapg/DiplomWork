using System.Threading.Tasks;
using DiplomaSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiplomaSolution.Services.Interfaces;

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
        public RegistrationController(ILogger<RegistrationController> logger, IRegistrationService registrationService)
        {
            Logger = logger;
            RegistrationService = registrationService;
        }

        #region DI services
        private ILogger<RegistrationController> Logger { get; set; }
        private IRegistrationService RegistrationService { get; set; }
        #endregion

        /// <summary>
        /// Add there a return url to proceed with the customers ...
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmationPage(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var registrationResult = await RegistrationService.CompleteRegistration(customer);

                if(registrationResult.StatusCode == StatusCodesEnum.RedirectNeeded && registrationResult.ValidationErrors.Count == 0) // check if we can pass data like this 
                {
                    return Redirect(Url.Action(registrationResult.ActionName, registrationResult.ControllerName, new { customerData = registrationResult.ResponseData }));
                }
                else if(registrationResult.StatusCode == StatusCodesEnum.BadDataProvided && registrationResult.ValidationErrors.Count > 0)
                {
                    foreach (var item in registrationResult.ValidationErrors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }

            return View("RegistrationForm");
        }

        /// <summary>
        /// Action to notify customer that he registered and need only to confirm email
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmationPage(string customerData)
        {
            return View("ConfirmationPage", customerData);
        }

        /// <summary>
        /// Action to confirm partner registration
        /// </summary>
        /// <param name="emailUrlConfirmation"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ConfirmPartnerRegister(string customerData)
        {
            return View("ConfirmationPage", customerData);
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
    }
}
