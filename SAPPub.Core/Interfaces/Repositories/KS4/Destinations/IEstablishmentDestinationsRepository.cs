using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Destinations
{
    public interface IEstablishmentDestinationsRepository
    {
        Task<IEnumerable<EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default);
        Task<EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default);
    }
}
