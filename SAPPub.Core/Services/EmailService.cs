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
        private ILogger<EmailService> _logger = logger ?? throw new ArgumentNullException(nameof(EmailService));

        public void SendGatewayEmail(string emailAddress, string localAuthorityName)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                _logger.LogError("Email address is null or empty. Cannot send gateway email.");
                throw new ArgumentException("Error sending email, missing argument");
            }
            if (string.IsNullOrWhiteSpace(localAuthorityName))
            {
                _logger.LogError("Local authority name is null or empty. Cannot send gateway email.");
                throw new ArgumentException("Error sending email, missing argument");
            }

            _emailRepository.SendGatewayEmail(emailAddress, localAuthorityName);
        }
    }
}
