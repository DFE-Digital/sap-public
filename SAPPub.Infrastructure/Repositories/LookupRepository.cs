using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class LookupRepository : ILookupRepository
    {
        private readonly IGenericRepository<Lookup> _repo;

        public LookupRepository(IGenericRepository<Lookup> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<Lookup>> GetAllLookupsAsync(CancellationToken ct = default)
            => await _repo.ReadAllAsync(ct);

        public async Task<Lookup?> GetLookupAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _repo.ReadAsync(id, ct);
        }
    }
}
