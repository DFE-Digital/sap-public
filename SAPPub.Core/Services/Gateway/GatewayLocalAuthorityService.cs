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
    public class GatewayLocalAuthorityService : IGatewayLocalAuthorityService
    {
        private readonly IGatewayLocalAuthorityRepository _authorityRepository;
        private readonly ILogger<GatewayUserService> _logger;

        public GatewayLocalAuthorityService(IGatewayLocalAuthorityRepository authorityRepository, ILogger<GatewayUserService> logger)
        {
            _authorityRepository = authorityRepository;
            _logger = logger;
        }

        public GatewayLocalAuthority? GetByName(string laName)
        {
            return _authorityRepository.GetByName(laName);

        }

        public GatewayLocalAuthority? GetById(Guid Id)
        {
            return _authorityRepository.GetById(Id);
        }

        public int CountSignUps(Guid Id)
        {
            return _authorityRepository.GetAll().Count(x => x.Id == Id);
        }

        public bool IsLAOpen(Guid Id)
        {
            var signUps = CountSignUps(Id);
            var maxSignUps = _authorityRepository.GetById(Id)?.MaxSessions ?? 0;
            return signUps >= maxSignUps;
        }
    }
}
