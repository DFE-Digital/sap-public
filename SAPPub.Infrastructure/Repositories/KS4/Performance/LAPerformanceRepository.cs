using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public sealed class LAPerformanceRepository : ILAPerformanceRepository
    {
        private readonly IGenericRepository<LAPerformance> _repo;
        private readonly ILogger<LAPerformanceRepository> _logger;

        public LAPerformanceRepository(
            IGenericRepository<LAPerformance> repo,
            ILogger<LAPerformanceRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<LAPerformance> GetAllLAPerformance()
        {
            // Keep only if genuinely used by the UI (can be large)
            return _repo.ReadAll() ?? Enumerable.Empty<LAPerformance>();
        }

        public LAPerformance GetLAPerformance(string laCode)
            => _repo.Read(laCode) ?? new LAPerformance();
    }
}
