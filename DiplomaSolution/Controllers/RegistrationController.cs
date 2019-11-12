using System;
using DiplomaSolution.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class RegistrationController : Controller
    {
        public RegistrationService RegistrationService { get; set; }

        public RegistrationController(RegistrationService registrationService)
        {
            RegistrationService = registrationService;
        }

        [HttpPost]
        public IActionResult Login(string emailAddress, string password)
        {
            // redirect -->
            if(RegistrationService.CheckRegistration(emailAddress))
            {
                RegistrationService.LogIn(emailAddress);
            }
            else
            {
                RegistrationService.Register(emailAddress, password);
            }

            return View();
        }
    }
}
