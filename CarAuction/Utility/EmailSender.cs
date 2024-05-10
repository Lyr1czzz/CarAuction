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
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = GetEmailBody(msg)
            };

            var client = new MailKit.Net.Smtp.SmtpClient();
            
            client.Connect("smtp.mail.ru", 465, SecureSocketOptions.Auto);
            client.Authenticate("carauction228@mail.ru", "RH4Jne0vRrvWCSgdh7uf");
            

            await client.SendAsync(message);
        }
        private string GetEmailBody(string link)
        {
            return new string($@"<body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">   <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 30px; border-radius: 10px;""><h2 style=""text-align: center; color: #333;"">Email Confirmation</h2><p style=""text-align: center; color: #666;"">Thank you for registering. Please confirm your email address by clicking the link below:</p><p style=""text-align: center;""><a href={link} style=""display: inline-block; background-color: #007bff; color: #ffffff; text-decoration: none; padding: 10px 20px; border-radius: 5px;"">Confirm Email</a></p><p style=""text-align: center; color: #999;"">If you did not request this email, you can safely ignore it.</p>    </div></body>");
        }
    }
}
