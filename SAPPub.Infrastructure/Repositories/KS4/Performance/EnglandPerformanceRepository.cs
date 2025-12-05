using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public class EnglandPerformanceRepository : IEnglandPerformanceRepository
    {
        private readonly IGenericRepository<EnglandPerformance> _EnglandPerformanceRepository;
        private ILogger<EnglandPerformance> _logger;

        public EnglandPerformanceRepository(
            IGenericRepository<EnglandPerformance> EnglandPerformanceRepository,
            ILogger<EnglandPerformance> logger)
        {
            _EnglandPerformanceRepository = EnglandPerformanceRepository;
            _logger = logger;
        }

        public EnglandPerformance GetEnglandPerformance()
        {
            return _EnglandPerformanceRepository.ReadAll()?.FirstOrDefault() ?? new EnglandPerformance();
        }
    }
}
