using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.Gateway
{
    public class GatewayUserService : IGatewayUserService
    {
        private readonly IGatewayUserRepository _gatewayUserRepository;
        private readonly ILogger<GatewayUserService> _logger;

        public GatewayUserService(IGatewayUserRepository gatewayUserRepository, ILogger<GatewayUserService> logger)
        {
            _gatewayUserRepository = gatewayUserRepository;
            _logger = logger;
        }

        public GatewayUser? GetByEmail(string email)
        {
            return _gatewayUserRepository.GetByEmail(email);
        }

        public GatewayUser? GetById(Guid? id)
        {
            return _gatewayUserRepository.GetById(id);
        }

        public Guid Insert(GatewayUser user)
        {
            if (string.IsNullOrWhiteSpace(user.EmailAddress))
            {
                throw new ArgumentException("Email address is required.", nameof(user.EmailAddress));
            }


            user.Id = Guid.NewGuid();
            user.RegisteredOn = DateTime.UtcNow;
            user.CreatedOn = DateTime.UtcNow;
            user.ModifiedOn = DateTime.UtcNow;
            user.IsDeleted = false;

            return _gatewayUserRepository.Insert(user) ? user.Id : throw new Exception("Error on Add user");

        }

        public IEnumerable<GatewayUser> GetAll()
        {
            return _gatewayUserRepository.GetAll();
        }
    }
}
