using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public class EmailService(IEmailRepository emailRepository, ILogger<EmailService> logger) : IEmailService
    {
        private readonly IEmailRepository _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        private readonly ILogger<EmailService> _logger = logger ?? throw new ArgumentNullException(nameof(EmailService));

        public void SendGatewayEmail(string emailAddress, string userId, string validationCheck)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                _logger.LogError("Email address is null or empty. Cannot send gateway email.");
                throw new ArgumentException("Error sending email, missing argument");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("UserId is null or empty. Cannot send gateway email.");
                throw new ArgumentException("Error sending email, missing argument");
            }

            if (string.IsNullOrWhiteSpace(validationCheck))
            {
                _logger.LogError("Validation check is null or empty. Cannot send gateway email.");
                throw new ArgumentException("Error sending email, missing argument");
            }

            _emailRepository.SendGatewayEmail(emailAddress, userId, validationCheck);
        }
    }
}
