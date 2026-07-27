using SAPPub.Core.Entities.Destinations;

namespace SAPPub.Core.Interfaces.Repositories.Destinations;

public interface IKS4DestinationsRepository
{
    Task<KS4EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default);
    Task<IEnumerable<KS4LADestinations>> GetAllLADestinationsAsync(CancellationToken ct = default);
    Task<KS4LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default);
    Task<IEnumerable<KS4EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default);
    Task<KS4EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<KS4EstablishmentDestinations>> GetEstablishmentsDestinationsAsync(IEnumerable<string> urns, CancellationToken ct = default);
}
