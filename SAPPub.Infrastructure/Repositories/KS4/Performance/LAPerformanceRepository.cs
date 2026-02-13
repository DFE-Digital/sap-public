using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public sealed class LAPerformanceRepository : ILAPerformanceRepository
    {
        private readonly IGenericRepository<LAPerformance> _repo;

        public LAPerformanceRepository(IGenericRepository<LAPerformance> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<LAPerformance>> GetAllLAPerformanceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<LAPerformance> GetLAPerformanceAsync(string laCode, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(laCode))
                return new LAPerformance();

            return await _repo.ReadAsync(laCode, ct) ?? new LAPerformance();
        }
    }
}
