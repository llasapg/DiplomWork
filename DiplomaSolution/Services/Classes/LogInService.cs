using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;

namespace DiplomaSolution.Services.Classes
{
    public class LogInService : ILogInService
    {
        private CustomerContext CustomerContext { get; set; }

        public LogInService(CustomerContext customerContext)
        {
            CustomerContext = customerContext;
        }

        public Customer LogIn(string email)
        {
            var resultCustomer = new Customer();

            foreach (var item in CustomerContext.Customers)
            {
                if (item.EmailAddress == email)
                {
                    resultCustomer = item;
                }
            }

            return resultCustomer;
        }
    }
}
