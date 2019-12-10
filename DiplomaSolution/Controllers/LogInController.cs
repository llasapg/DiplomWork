using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class LogInController : Controller
    {
        public IRegistrationService RegistrationService { get; set; }

        public LogInController(IRegistrationService logInService)
        {
            RegistrationService = logInService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
