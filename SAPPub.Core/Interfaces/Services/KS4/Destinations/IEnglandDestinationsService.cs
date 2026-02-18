using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Services.KS4.Destinations
{
    public interface IEnglandDestinationsService
    {
        Task<EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default);
    }
}
