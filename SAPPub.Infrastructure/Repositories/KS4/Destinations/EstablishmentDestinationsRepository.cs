using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public sealed class EstablishmentDestinationsRepository : IEstablishmentDestinationsRepository
    {
        private readonly IGenericRepository<EstablishmentDestinations> _repo;
        private readonly ILogger<EstablishmentDestinationsRepository> _logger;

        public EstablishmentDestinationsRepository(
            IGenericRepository<EstablishmentDestinations> repo,
            ILogger<EstablishmentDestinationsRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Keep only if genuinely used (can be huge)
        public IEnumerable<EstablishmentDestinations> GetAllEstablishmentDestinations()
        {
            try
            {
                return _repo.ReadAll() ?? Enumerable.Empty<EstablishmentDestinations>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load all establishment destinations");
                return Enumerable.Empty<EstablishmentDestinations>();
            }
        }

        public EstablishmentDestinations? GetEstablishmentDestinations(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return null;

            try
            {
                return _repo.Read(urn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load establishment destinations for URN={Urn}", urn);
                return null;
            }
        }
    }
}
