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
    public sealed class GatewayLocalAuthorityRepository(
        IGenericRepository<GatewayLocalAuthority> genericRepository,
        ILogger<Establishment> logger) : IGatewayLocalAuthorityRepository
    {
        private readonly IGenericRepository<GatewayLocalAuthority> _genericRepository = genericRepository ?? throw new ArgumentNullException(nameof(genericRepository));
        private ILogger<Establishment> _logger = logger ?? throw new ArgumentNullException(nameof(genericRepository));

        public async Task<GatewayLocalAuthority?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _genericRepository.ReadSingleAsync(new { Id = id }, ct);
        }

        public async Task<IEnumerable<GatewayLocalAuthority>> GetAllAsync(CancellationToken ct = default)
        {
            return await _genericRepository.ReadAllAsync() ?? [];
        }
    }
}
