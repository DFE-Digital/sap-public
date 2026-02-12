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
    public class GatewayUserAuditService : IGatewayUserAuditService
    {
        private readonly IGatewayUserAuditRepository _gatewayUserAuditRepository;
        private readonly ILogger<GatewayUserAuditService> _logger;

        public GatewayUserAuditService(IGatewayUserAuditRepository gatewayUserAuditRepository, ILogger<GatewayUserAuditService> logger)
        {
            _gatewayUserAuditRepository = gatewayUserAuditRepository;
            _logger = logger;
        }

        public void Insert(Guid userId)
        {
            _gatewayUserAuditRepository.Insert(new GatewayUserAudit
            {
                UserId = userId,
                LoginDateTime = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                ModifiedOn = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
        }
    }
}
