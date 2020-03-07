using System.Threading.Tasks;
using DiplomaSolution.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Main method to perform customer login in our system
        /// </summary>
        Task<IActionResult> LoginCustomer(LoginViewModel customer, string returnUrl = null);
    }
}
