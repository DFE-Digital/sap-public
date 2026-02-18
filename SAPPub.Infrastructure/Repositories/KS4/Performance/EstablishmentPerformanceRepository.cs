using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public sealed class EstablishmentPerformanceRepository : IEstablishmentPerformanceRepository
    {
        private readonly IGenericRepository<EstablishmentPerformance> _repo;

        public EstablishmentPerformanceRepository(IGenericRepository<EstablishmentPerformance> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EstablishmentPerformance>> GetAllEstablishmentPerformanceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new EstablishmentPerformance();

            return await _repo.ReadAsync(urn, ct) ?? new EstablishmentPerformance();
        }
    }
}
