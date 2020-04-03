using System.Threading.Tasks;
using DiplomaSolution.Models;

namespace DiplomaSolution.Services.Interfaces
{
    /// <summary>
    /// Service with logic with registration customers
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Method to perform customer registration in case when he provided to us valid data
        /// </summary>
        /// <returns></returns>
        Task<DefaultServiceResponse> CompleteRegistration(CustomerViewModel customer);
    }
}
