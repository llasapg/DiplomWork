using System;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using SendGrid;

namespace DiplomaSolution.Services.Interfaces
{
    public interface ISendEmailService
    {
         Task<Response> SendEmail(ServiceEmail serviceEmail);
    }
}
