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
    public class GatewayPageViewAuditService : IGatewayPageViewAuditService
    {
        private readonly IGatewayPageViewAuditRepository _gatewayPageViewAuditRepository;
        private readonly ILogger<GatewayPageViewAuditService> _logger;

        public GatewayPageViewAuditService(IGatewayPageViewAuditRepository gatewayPageViewAuditRepository, ILogger<GatewayPageViewAuditService> logger)
        {
            _gatewayPageViewAuditRepository = gatewayPageViewAuditRepository;
            _logger = logger;
        }

        public void Insert(Guid? userId, string page)
        {
            if (string.IsNullOrWhiteSpace(page))
            {
                _logger.LogError("Page URL has no value");
            }
            else
            {
                _gatewayPageViewAuditRepository.Insert(new GatewayPageViewAudit
                {
                    UserId = userId,
                    Url = page,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    ModifiedOn = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                });
            }
        }
    }
}
