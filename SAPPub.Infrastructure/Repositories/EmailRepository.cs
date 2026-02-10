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

        public EmailRepository(INotificationClient notificationClient, IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
            _notificationClient = notificationClient;

        }

        public void SendGatewayEmail(string emailAddress)
        {
            var personalisation = new Dictionary<string, dynamic>
            {
                { "link", "https://www.localhost:3000/welcome/returning" }
            };
            var response = _notificationClient.SendEmail(emailAddress, _emailOptions.GatewayTemplate, personalisation);
        }
    }
}
