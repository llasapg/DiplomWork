using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using System.Linq;

namespace DiplomaSolution.Services.Classes
{
    public class LogInService : ILogInService
    {
        private CustomerContext CustomerContext { get; set; }

        public LogInService(CustomerContext customerContext)
        {
            CustomerContext = customerContext;
        }

        /// <summary>
        /// finds our customer by email and gives it back
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
