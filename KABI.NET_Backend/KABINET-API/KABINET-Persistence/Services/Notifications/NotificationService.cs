using KABINET_Application.Boundaries.Notification;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KABINET_Persistance.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration configuration;

        public NotificationService(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    this.configuration["NotificationServiceCredentials:EmailAdress"],
                    this.configuration["NotificationServiceCredentials:Password"])
            };

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pu-kabinet@admin.com");
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            client.EnableSsl = true;

            await client.SendMailAsync(mailMessage);
        }
    }
}
