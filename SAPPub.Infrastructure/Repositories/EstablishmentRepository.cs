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

        public Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default)
        {
            // Keep only while we genuinely need to list; LIMIT 100 is already in DapperHelpers
            return _repo.ReadAllAsync(ct);
        }

        public async Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new Establishment();

            return await _repo.ReadAsync(urn, ct) ?? new Establishment();
        }
    }
}
