using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public sealed class EnglandPerformanceRepository : IEnglandPerformanceRepository
    {
        private readonly IGenericRepository<EnglandPerformance> _repo;
        private readonly ILogger<EnglandPerformanceRepository> _logger;

        public EnglandPerformanceRepository(
            IGenericRepository<EnglandPerformance> repo,
            ILogger<EnglandPerformanceRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<EnglandPerformance> GetAllEnglandPerformance()
        {
            return _repo.ReadAll() ?? Enumerable.Empty<EnglandPerformance>();
        }

        public EnglandPerformance GetEnglandPerformance()
            => _repo.ReadSingle(new { }) ?? new EnglandPerformance();
    }
}
