using System.Linq;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;

namespace DiplomaSolution.Services // Should be deleted
{
    public class RegistrationService : IRegistrationService
    {
        public CustomerContext CustomerContext { get; set; }

        public RegistrationService(CustomerContext customerContext)
        {
            CustomerContext = customerContext;
        }

        public bool CheckRegistration(Customer customer) // Check in DB6 if customer is registered
        {
            var allCustomersList = CustomerContext.Customers.ToList();

            var registerCheck = false;

            foreach (var item in allCustomersList)
            {
                if (item.EmailAddress == customer.EmailAddress)
                {
                    registerCheck = true;
                }
            }

            return registerCheck;
        }

        public bool LogIn(Customer customer)
        {
            if (CheckRegistration(customer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(Customer customer)
        {
            if (!CheckRegistration(customer))
            {
                CustomerContext.Customers.Add(
                    new Customer
                    {
                        EmailAddress = customer.EmailAddress,
                        Password = customer.Password,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName
                    });

                CustomerContext.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
