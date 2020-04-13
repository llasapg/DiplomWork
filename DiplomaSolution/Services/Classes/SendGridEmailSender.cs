using System.Threading.Tasks;
using DiplomaSolution.ConfigurationModels;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DiplomaSolution.Services.Classes
{
    public class SendGridEmailSender : ISendEmailService
    {
        /// <summary>
        /// Set of all configurations need to send emails via SendGrid
        /// </summary>
        private IOptionsSnapshot<Authentication> Configurartions { get; set; }

        public SendGridEmailSender(IOptionsSnapshot<Authentication> configurartions)
        {
            Configurartions = configurartions;
        }

        /// <summary>
        /// Method to send email via SendGrid 3-rd party
        /// </summary>
        /// <param name="serviceEmail"></param>
        /// <returns></returns>
        public async Task<Response> SendEmail(ServiceEmail serviceEmail) //todo - create custom message formatter to make it look better
        {
            var client = new SendGridClient(Configurartions.Value.SendGridAuthentication.ApiKey);
            var from = new EmailAddress(serviceEmail.FromEmail, serviceEmail.FromName);
            var subject = serviceEmail.EmailSubject;
            var to = new EmailAddress(serviceEmail.ToEmail, serviceEmail.ToName);
            var plainTextContent = serviceEmail.EmailText;
            var htmlContent = serviceEmail.EmailHtmlText;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
