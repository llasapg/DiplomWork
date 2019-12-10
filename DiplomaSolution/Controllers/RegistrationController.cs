using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
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

        public IActionResult ConfirmationPage(Customer customer)
        {
            CreateNewAccount(customer);

            ViewBag.FirstName = customer.FirstName;

            return View();
        }

        public IActionResult RegistrationForm()
        {
            return View();
        }
    }
}
