using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Toolkit_API.Application.Interfaces;
namespace Toolkit_API.Infrastructure.Services
{
    public class EmailServices : IEmailServices
    {
        public async Task<string> SendEmail(string to, string subject, string body)
        {
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress("Avtoolkit News Letter", "avtoolkitnews@gmail.com"));

            msg.To.Add(new MailboxAddress("", to));

            msg.Subject = subject;
            msg.Body = new TextPart("plain")
            {
                Text = body
            };

            var pS = Environment.GetEnvironmentVariable("NEWS_PS") ?? throw new InvalidOperationException("'NEWS_PS' not found");

            try
            {
                using (var client = new SmtpClient())
                {

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("avtoolkitnews@gmail.com", pS);
                    await client.SendAsync(msg);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {

                return $"Failed to send email: {ex.Message}";

            }

            return "Email sent successfully";
        }
        public async Task<string> SubscribeToNewsLetter(string email)
        {
            // Logic to add the email to the subscription list
            return "Subscribed successfully";
        }
    }
}
