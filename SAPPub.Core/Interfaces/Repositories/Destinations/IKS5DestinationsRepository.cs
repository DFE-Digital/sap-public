using SAPPub.Core.Entities.Destinations;

namespace SAPPub.Core.Interfaces.Repositories.Destinations;

public interface IKS5DestinationsRepository
{
    Task<KS5EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default);
    Task<KS5LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default);
    Task<KS5EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default);
}