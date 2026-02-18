using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public sealed class EnglandPerformanceRepository : IEnglandPerformanceRepository
    {
        private readonly IGenericRepository<EnglandPerformance> _repo;

        public EnglandPerformanceRepository(IGenericRepository<EnglandPerformance> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EnglandPerformance>> GetAllEnglandPerformanceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default)
        {
            // Single-row view: v_england_performance
            return await _repo.ReadSingleAsync(new { }, ct) ?? new EnglandPerformance();
        }
    }
}
