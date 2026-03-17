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
    public class GatewayUserRepository : IGatewayUserRepository
    {
        private readonly IGenericRepository<GatewayUser> _genericRepository;
        private ILogger<Establishment> _logger;

        public GatewayUserRepository(
            IGenericRepository<GatewayUser> genericRepository,
            ILogger<Establishment> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public async Task<GatewayUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _genericRepository.ReadSingleAsync(new { Id = id }, ct);
        }

        public async Task<bool> InsertAsync(GatewayUser user)
        {
            return await _genericRepository.WriteAsync(user);
        }

        public async Task<IEnumerable<GatewayUser>> GetAllAsync(CancellationToken ct = default)
        {
            return await _genericRepository.ReadAllAsync(ct);
        }
    }
}
