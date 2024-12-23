using Shop_Core.Interfaces;
using Shop_Core.Models;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Shop_Infrastructure.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        // Constructor to inject dependencies
        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void SendEmail(Email em)
        {
            try
            {
                // Retrieve email credentials and SMTP settings from configuration
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];

                var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword)
                };

                // Create the email message
                var message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = em.Subject,
                    Body = em.Body,
                    IsBodyHtml = true
                };

                message.To.Add(new MailAddress(em.Recivers));

                // Send the email
                client.Send(message);

                _logger.LogInformation("Email sent successfully to {0}", em.Recivers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                throw; // Optionally rethrow the exception or handle it as needed
            }
        }
    }
}
