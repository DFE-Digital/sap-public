using Microsoft.Extensions.Logging;
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
    public class EstablishmentPerformanceRepository : IEstablishmentPerformanceRepository
    {
        private readonly IGenericRepository<EstablishmentPerformance> _establishmentPerformanceRepository;
        private ILogger<EstablishmentPerformance> _logger;

        public EstablishmentPerformanceRepository(
            IGenericRepository<EstablishmentPerformance> establishmentPerformanceRepository,
            ILogger<EstablishmentPerformance> logger)
        {
            _establishmentPerformanceRepository = establishmentPerformanceRepository;
            _logger = logger;
        }


        public IEnumerable<EstablishmentPerformance> GetAllEstablishmentPerformance()
        {
            return _establishmentPerformanceRepository.ReadAll() ?? [];
        }


        public EstablishmentPerformance GetEstablishmentPerformance(string urn)
        {
            return GetAllEstablishmentPerformance().FirstOrDefault(x => x.Id == urn) ?? new EstablishmentPerformance();
        }
    }
}
