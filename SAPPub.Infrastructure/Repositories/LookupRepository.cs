using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class LookupRepository : ILookupRepository
    {
        private readonly IGenericRepository<Lookup> _repo;
        private readonly ILogger<LookupRepository> _logger;

        public LookupRepository(
            IGenericRepository<Lookup> repo,
            ILogger<LookupRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Lookup>> GetAllLookupsAsync(CancellationToken ct = default)
            => await _repo.ReadAllAsync(ct) ?? Enumerable.Empty<Lookup>();

        public async Task<Lookup?> GetLookupAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _repo.ReadAsync(id, ct);
        }
    }
}
