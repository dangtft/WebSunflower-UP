using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using WebShop.Areas.Admin.Interface;

namespace WebShop.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SenderEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("haidang29072000@gmail.com", "ovtawfvewrwnript")
            };

            return client.SendMailAsync(
                new MailMessage(from: "haidang29072000@gmail.com",
                to:email,
                subject,
                message
                ));
        }
    }
}
