using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Services.KS4.Destinations
{
    public interface IEstablishmentDestinationsService
    {
        Task<IEnumerable<EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default);
        Task<EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default);
    }
}
