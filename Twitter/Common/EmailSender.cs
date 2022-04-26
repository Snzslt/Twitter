using System.Configuration;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Twitter.Common
{
    public class MyEmailSender 
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {

            var fromEmail = "twitter.app22@gmail.com";
            var fromPassword = "twitterapp22@";

            var fromAddress = new MailAddress(fromEmail, "twitter");
            var toAddress = new MailAddress(email, "email");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
