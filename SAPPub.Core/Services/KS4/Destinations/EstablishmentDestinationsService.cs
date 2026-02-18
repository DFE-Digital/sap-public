using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4.Destinations
{
    public sealed class EstablishmentDestinationsService : IEstablishmentDestinationsService
    {
        private readonly IEstablishmentDestinationsRepository _repo;

        public EstablishmentDestinationsService(IEstablishmentDestinationsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default)
        {
            return await _repo.GetAllEstablishmentDestinationsAsync(ct);
        }

        public async Task<EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default)
        {
            return await _repo.GetEstablishmentDestinationsAsync(urn, ct);
        }
    }
}
