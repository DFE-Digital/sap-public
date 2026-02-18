using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Destinations
{
    public interface ILADestinationsRepository
    {
        Task<IEnumerable<LADestinations>> GetAllLADestinationsAsync(CancellationToken ct = default);
        Task<LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default);
    }
}
