using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public class EmailService(IEmailRepository emailRepository) : IEmailService
    {
        private readonly IEmailRepository _emailRepository = emailRepository;

        public void SendGatewayEmail(string emailAddress)
        {
            _emailRepository.SendGatewayEmail(emailAddress);
        }
    }
}
