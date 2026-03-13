using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Interfaces;
using Notify.Models;
using Notify.Models.Responses;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Web.Models.Config;
using System.Net.Mail;

namespace SAPPub.Infrastructure.Repositories
{
    public  class EmailRepository : IEmailRepository
    {
        private readonly INotificationClient _notificationClient;
        private readonly EmailOptions _emailOptions;
        private readonly ILogger<EmailRepository> _logger;

        public EmailRepository(INotificationClient notificationClient, IOptions<EmailOptions> emailOptions, ILogger<EmailRepository> logger)
        {
            _emailOptions = emailOptions.Value;
            _notificationClient = notificationClient;
            _logger = logger;
        }

        public void SendGatewayEmail(string emailAddress)
        {
            var personalisation = new Dictionary<string, dynamic>
            {
                { "link", "https://www.localhost:3000/welcome/returning" }
            };
            try
            {
                _logger.LogError($"Attempting to send gateway email of type {_emailOptions.GatewayTemplate}");
                var response = _notificationClient.SendEmail(emailAddress, _emailOptions.GatewayTemplate, personalisation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to send gateway email of type {_emailOptions.GatewayTemplate}");
            }
            
        }
    }
}
