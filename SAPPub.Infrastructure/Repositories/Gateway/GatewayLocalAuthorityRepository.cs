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
    public sealed class GatewayLocalAuthorityRepository : IGatewayLocalAuthorityRepository
    {
        private readonly IGenericRepository<GatewayLocalAuthority> _genericRepository;
        private ILogger<Establishment> _logger;

        public GatewayLocalAuthorityRepository(
            IGenericRepository<GatewayLocalAuthority> genericRepository,
            ILogger<Establishment> logger)
        {
            _genericRepository = genericRepository ?? throw new ArgumentNullException(nameof(genericRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(genericRepository));
        }

        public async Task<GatewayLocalAuthority?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _genericRepository.ReadAsync(id.ToString(), ct);
        }

        public async Task<IEnumerable<GatewayLocalAuthority>> GetAllAsync(CancellationToken ct = default)
        {
            return await _genericRepository.ReadAllAsync() ?? new List<GatewayLocalAuthority>();
        }
    }
}
