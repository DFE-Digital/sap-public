using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Services.KS4.Destinations
{
    public interface ILADestinationsService
    {
        Task<IEnumerable<LADestinations>> GetAllLADestinationsAsync(CancellationToken ct = default);
        Task<LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default);
    }
}
