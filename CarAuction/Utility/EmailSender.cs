using System.Net.Mail;
using System.Net;

namespace CarAuction.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 465)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("travistest105@gmail.com", "43gjhHkgT53tg")
            };

            return client.SendMailAsync(
                new MailMessage(from: "travistest105@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
