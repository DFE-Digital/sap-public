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
    public  class EmailRepository(INotificationClient notificationClient, IOptions<EmailOptions> emailOptions, ILogger<EmailRepository> logger) : IEmailRepository
    {
        private readonly INotificationClient _notificationClient = notificationClient;
        private readonly EmailOptions _emailOptions = emailOptions.Value;
        private readonly ILogger<EmailRepository> _logger = logger;

        public void SendGatewayEmail(string emailAddress, string userId, string validationCheck)
        {
            var personalisation = new Dictionary<string, dynamic>
            {
                { "link", $"{_emailOptions.ApplicationRoot}/gateway/link/{userId}?validate={validationCheck}" }
            };
            try
            {
                _logger.LogInformation($"Attempting to send gateway email of type {_emailOptions.GatewayTemplate}");
                var response = _notificationClient.SendEmail(emailAddress, _emailOptions.GatewayTemplate, personalisation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to send gateway email of type {_emailOptions.GatewayTemplate}");
            }
            
        }
    }
}
