using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Repositories.KS4.Performance
{
    public sealed class EstablishmentPerformanceRepository : IEstablishmentPerformanceRepository
    {
        private readonly IGenericRepository<EstablishmentPerformance> _repo;
        private readonly ILogger<EstablishmentPerformanceRepository> _logger;

        public EstablishmentPerformanceRepository(
            IGenericRepository<EstablishmentPerformance> repo,
            ILogger<EstablishmentPerformanceRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<EstablishmentPerformance> GetAllEstablishmentPerformance()
        {
            return _repo.ReadAll() ?? Enumerable.Empty<EstablishmentPerformance>();
        }

        public EstablishmentPerformance GetEstablishmentPerformance(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new EstablishmentPerformance();

            return _repo.Read(urn) ?? new EstablishmentPerformance();
        }
    }
}
