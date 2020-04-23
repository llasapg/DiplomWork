using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.ViewModels;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Main method to perform customer login in our system
        /// </summary>
        Task<DefaultServiceResponse> LoginCustomer(LoginViewModel customer, string returnUrl = null);

        /// <summary>
        /// Callback method to login customer in case if he decided to login usin external provider
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        Task<DefaultServiceResponse> ExternalLoginCallBack(string returnUrl = null, string remoteError = null);
    }
}
