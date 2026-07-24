using SAPPub.Core.ServiceModels.Destinations;

namespace SAPPub.Core.Interfaces.Services;

public interface IDestinationsService
{
    Task<KS4DestinationsDetails> GetKS4DestinationsDetailsAsync(string urn, CancellationToken ct = default);
    Task<KS5DestinationsDetails> GetKS5DestinationsDetailsAsync(string urn, CancellationToken ct = default);
}
