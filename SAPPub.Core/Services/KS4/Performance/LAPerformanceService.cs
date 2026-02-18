using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance
{
    public sealed class LAPerformanceService : ILAPerformanceService
    {
        private readonly ILAPerformanceRepository _repo;

        public LAPerformanceService(ILAPerformanceRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<LAPerformance>> GetAllLAPerformanceAsync(CancellationToken ct = default)
        {
            return await _repo.GetAllLAPerformanceAsync(ct);
        }

        public async Task<LAPerformance> GetLAPerformanceAsync(string laCode, CancellationToken ct = default)
        {
            return await _repo.GetLAPerformanceAsync(laCode, ct);
        }
    }
}
