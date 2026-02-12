using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.Gateway
{
    public class GatewayPageViewAuditRepository : IGatewayPageViewAuditRepository
    {
        private readonly IGenericCRUDRepository<GatewayPageViewAudit> _genericRepository;
        private ILogger<Establishment> _logger;

        public GatewayPageViewAuditRepository(
            IGenericCRUDRepository<GatewayPageViewAudit> genericRepository,
            ILogger<Establishment> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public bool Insert(GatewayPageViewAudit user)
        {
            return _genericRepository.Create(user);
        }
    }
}
