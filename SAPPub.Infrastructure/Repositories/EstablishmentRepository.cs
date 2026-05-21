using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly IGenericRepository<Establishment> _repo;

        public EstablishmentRepository(IGenericRepository<Establishment> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
        {
            return _repo.ReadPageAsync(page, take, ct);
        }

        public async Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return null;

            return await _repo.ReadAsync(urn, ct) ?? null;
        }
    }
}
