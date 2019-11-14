using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    [Route("LogIn")]
    public class LogInController : Controller
    {
        public IRegistrationService RegistrationService { get; set; }

        public LogInController(IRegistrationService logInService)
        {
            RegistrationService = logInService;
        }

        [Route("SignIn")]
        public IActionResult SignIn(Customer customer)
        {
            if(RegistrationService.CheckRegistration(customer))
            {
                return View();
            }
            else
            {
                return View("~/Registration/CreateNewAccount");
            }
        }
    }
}
