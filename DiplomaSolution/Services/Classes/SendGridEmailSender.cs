using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DiplomaSolution.Services.Classes
{
    public class SendGridEmailSender : ISendEmailService
    {
        public async Task<Response> SendEmail(ServiceEmail serviceEmail)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
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
