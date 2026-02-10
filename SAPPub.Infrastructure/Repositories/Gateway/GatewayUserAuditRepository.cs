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
    public class GatewayUserAuditRepository : IGatewayUserAuditRepository
    {
        private readonly IGenericCRUDRepository<GatewayUserAudit> _genericRepository;
        private ILogger<Establishment> _logger;

        public GatewayUserAuditRepository(
            IGenericCRUDRepository<GatewayUserAudit> genericRepository,
            ILogger<Establishment> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public bool Insert(GatewayUserAudit user)
        {
            return _genericRepository.Create(user);
        }
    }
}
