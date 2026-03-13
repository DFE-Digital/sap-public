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

        public async Task<GatewayLocalAuthority?> GetByName(string laName)
        {
            var allLas = await _authorityRepository.GetAllAsync();
            var thisLa = allLas.Where(x => x.LocalAuthorityName.Equals(laName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return thisLa ?? throw new Exception($"LA with name {laName} not found.");
        }

        public async Task<GatewayLocalAuthority?> GetById(Guid Id)
        {
            return await _authorityRepository.GetByIdAsync(Id);
        }

        public int CountSignUps(Guid Id)
        {
            var localAuth = GetById(Id).Result ?? throw new InvalidOperationException($"Local authority with ID {Id} not found.");
            return localAuth.MaxSessions;  
        }

        public async Task<bool> IsLAOpen(Guid Id)
        {
            var signUps = CountSignUps(Id);

            var authority = await _authorityRepository.GetByIdAsync(Id) ?? throw new InvalidOperationException($"Local authority with ID {Id} not found.");
            var maxSignUps = authority.MaxSessions;
            return signUps >= maxSignUps;
        }
    }
}
