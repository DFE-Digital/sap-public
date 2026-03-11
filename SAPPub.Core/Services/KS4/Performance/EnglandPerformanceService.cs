using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance
{
    public sealed class EnglandPerformanceService : IEnglandPerformanceService
    {
        private readonly IEnglandPerformanceRepository _repo;

        public EnglandPerformanceService(IEnglandPerformanceRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default)
        {
            return await _repo.GetEnglandPerformanceAsync(ct);
        }
    }
}
