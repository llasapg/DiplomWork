using DiplomaSolution.Models;

namespace DiplomaSolution.Services.Interfaces
{
    public interface ILogInService
    {
        Customer LogIn(string email);
    }
}
