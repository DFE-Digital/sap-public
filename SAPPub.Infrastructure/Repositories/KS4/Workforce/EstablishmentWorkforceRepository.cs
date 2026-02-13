using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;

namespace SAPPub.Infrastructure.Repositories.KS4.Workforce
{
    public sealed class EstablishmentWorkforceRepository : IEstablishmentWorkforceRepository
    {
        private readonly IGenericRepository<EstablishmentWorkforce> _repo;
        private readonly ILogger<EstablishmentWorkforceRepository> _logger;

        public EstablishmentWorkforceRepository(
            IGenericRepository<EstablishmentWorkforce> repo,
            ILogger<EstablishmentWorkforceRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<EstablishmentWorkforce> GetAllEstablishmentWorkforce()
        {
            // Keep only if genuinely used (can be large)
            return _repo.ReadAll() ?? Enumerable.Empty<EstablishmentWorkforce>();
        }

        public EstablishmentWorkforce GetEstablishmentWorkforce(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new EstablishmentWorkforce();

            return _repo.Read(urn) ?? new EstablishmentWorkforce();
        }
    }
}
