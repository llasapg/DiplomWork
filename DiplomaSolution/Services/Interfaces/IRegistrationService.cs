using System;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IRegistrationService
    {
        bool CheckRegistration(string emailAddress);

        bool LogIn(string emailAddress);

        bool Register(string emailAddress, string password);
    }
}
