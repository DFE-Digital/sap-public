using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Services.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.Gateway
{
    public class GateWayUserLAService : IGatewayUserLAService
    {
        private readonly IGatewayUserRepository _gatewayUserRepository;
        private readonly IGatewayLocalAuthorityRepository _authorityRepository;
        private readonly ILogger<GatewayUserService> _logger;

        public GateWayUserLAService(IGatewayUserRepository gatewayUserRepository, IGatewayLocalAuthorityRepository localAuthorityRepository, ILogger<GatewayUserService> logger)
        {
            _gatewayUserRepository = gatewayUserRepository;
            _authorityRepository = localAuthorityRepository;
            _logger = logger;
        }

        public bool CanRegisterNewUsers(Guid laId)
        {
            var laDetails = _authorityRepository.GetById(laId);
            if (laDetails == null)
            {
                return false;
            }
            var currentUserCount = _gatewayUserRepository.GetAll().Count(x => x.LocalAuthorityId == laId);
            return currentUserCount < laDetails.MaxSessions;
        }
    }
}
