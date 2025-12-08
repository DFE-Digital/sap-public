using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.KS4.Performance
{
    public class LAPerformanceService : ILAPerformanceService
    {
        private readonly ILAPerformanceRepository _LAPerformanceRepository;

        public LAPerformanceService(ILAPerformanceRepository LAPerformanceRepository)
        {
            _LAPerformanceRepository = LAPerformanceRepository;
        }


        public IEnumerable<LAPerformance> GetAllLAPerformance()
        {
            return _LAPerformanceRepository.GetAllLAPerformance();
        }


        public LAPerformance GetLAPerformance(string urn)
        {
            return _LAPerformanceRepository.GetLAPerformance(urn) ?? new();
        }
    }
}
