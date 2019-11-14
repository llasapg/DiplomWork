using DiplomaSolution.Models;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IRegistrationService
    {
        bool CheckRegistration(Customer customer);

        bool LogIn(Customer customer);

        bool Register(Customer customer);
    }
}
