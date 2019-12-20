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

        [HttpPost]
        public IActionResult ConfirmationPage(Customer customer)
        {
            if (ModelState.IsValid)
            {
                CreateNewAccount(customer);

                ViewBag.FirstName = customer.FirstName;

                return View();
            }
            else
            {
                return View("RegistrationForm");
            }
        }

        [HttpGet]
        public IActionResult RegistrationForm()
        {
            return View();
        }
    }
}
