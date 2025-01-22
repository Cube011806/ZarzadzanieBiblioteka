namespace ZarzadzanieBiblioteka.Models
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides email sending functionality for the library management system.
    /// </summary>
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings for the email service.</param>
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The body of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(
                _configuration["SmtpSettings:SenderName"],
                _configuration["SmtpSettings:SenderEmail"]
            ));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _configuration["SmtpSettings:Server"],
                    int.Parse(_configuration["SmtpSettings:Port"]),
                    bool.Parse(_configuration["SmtpSettings:UseSsl"])
                );

                await client.AuthenticateAsync(
                    _configuration["SmtpSettings:Username"],
                    _configuration["SmtpSettings:Password"]
                );

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
