using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4.Destinations;

public sealed class EstablishmentDestinationsService(IEstablishmentDestinationsRepository repo) : IEstablishmentDestinationsService
{
    public Task<IEnumerable<EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default)
    {
        return repo.GetAllEstablishmentDestinationsAsync(ct);
    }

    public Task<EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default)
    {
        return repo.GetEstablishmentDestinationsAsync(urn, ct);
    }

    public Task<IEnumerable<EstablishmentDestinations>> GetEstablishmentDestinationsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        return repo.GetEstablishmentsDestinationsAsync(urns, ct);
    }
}
