using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance
{
    public sealed class EstablishmentPerformanceService : IEstablishmentPerformanceService
    {
        private readonly IEstablishmentPerformanceRepository _repo;

        public EstablishmentPerformanceService(IEstablishmentPerformanceRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IEnumerable<EstablishmentPerformance>> GetAllEstablishmentPerformanceAsync(CancellationToken ct = default)
        {
            return _repo.GetAllEstablishmentPerformanceAsync(ct);
        }

        public Task<EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
        {
            return _repo.GetEstablishmentPerformanceAsync(urn, ct);
        }
    }
}
