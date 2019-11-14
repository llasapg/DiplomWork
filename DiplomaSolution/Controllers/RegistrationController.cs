using System;
using DiplomaSolution.Models;
using DiplomaSolution.Services;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    [Route("Registration")]
    public class RegistrationController : Controller
    {
        public IRegistrationService RegistrationService { get; set; }

        public RegistrationController(IRegistrationService registrationService)
        {
            RegistrationService = registrationService;
        }

        public bool CreateNewAccount(Customer customer)
        {
            RegistrationService.Register(customer);

            return RegistrationService.CheckRegistration(customer);
        }

        [Route("ConfirmationPage")]
        public IActionResult ConfirmationPage(Customer customer)
        {
            CreateNewAccount(customer);

            ViewBag.FirstName = customer.FirstName;

            return View();
        }

        [Route("RegistrationForm")]
        public IActionResult RegistrationForm()
        {
            return View();
        }
    }
}
