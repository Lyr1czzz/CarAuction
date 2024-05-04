using System.Net.Mail;
using System.Net;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace CarAuction.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string msg, string registrationCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CarAuction", "carauction228@mail.ru"));
            message.To.Add(new MailboxAddress("лох бобовый", email));
            message.Subject = "Регистрация";
            message.Body = new TextPart("plain")
            {
                Text = msg
            };

            var client = new MailKit.Net.Smtp.SmtpClient();
            
            client.Connect("smtp.mail.ru", 465, SecureSocketOptions.Auto);
            client.Authenticate("carauction228@mail.ru", "RH4Jne0vRrvWCSgdh7uf");
            

            await client.SendAsync(message);
        }
    }
}
