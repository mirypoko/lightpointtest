using System.Threading.Tasks;
using Domain.ServiceModels;
using MailKit.Net.Smtp;
using MimeKit;
using Servises.Interfaces;

namespace MailKitEmailService
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message, MailOptions mailOptions)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(mailOptions.SenderName, mailOptions.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailOptions.SmtpHost, mailOptions.SmtpPort, mailOptions.UseSsl);
                await client.AuthenticateAsync(mailOptions.SmtpUserName, mailOptions.SmtpPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
