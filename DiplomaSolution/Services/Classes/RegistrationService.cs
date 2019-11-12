using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace DiplomaSolution.Services
{
    public class RegistrationService : IRegistrationService
    {
        public CustomerContext CustomerContext { get; set; }

        public RegistrationService(CustomerContext customerContext)
        {
            CustomerContext = customerContext;
        }

        public bool CheckRegistration(string emailAddress) // Check in DB6 if customer is registered
        {
            var allCustomersList = CustomerContext.Customers.ToList();

            var registerCheck = false;

            foreach (var item in allCustomersList)
            {
                if (item.EmailAddress == emailAddress)
                {
                    registerCheck = true;
                }
            }

            return registerCheck;
        }

        public bool LogIn(string emailAddress)
        {
            if(CheckRegistration(emailAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(string emailAddress, string password)
        {
            if(!CheckRegistration(emailAddress))
            {
                CustomerContext.Customers.Add(new Customer { EmailAddress = emailAddress, Password = password });
                return true;
            }
            else
            {
                return false; 
            }
        }
    }
}
