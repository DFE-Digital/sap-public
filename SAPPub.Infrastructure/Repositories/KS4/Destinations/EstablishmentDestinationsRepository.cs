using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public sealed class EstablishmentDestinationsRepository : IEstablishmentDestinationsRepository
    {
        private readonly IGenericRepository<EstablishmentDestinations> _repo;

        public EstablishmentDestinationsRepository(IGenericRepository<EstablishmentDestinations> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return null;

            return await _repo.ReadAsync(urn, ct);
        }
    }
}
