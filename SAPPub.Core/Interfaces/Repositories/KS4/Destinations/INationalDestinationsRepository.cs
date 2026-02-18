using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Destinations
{
    public interface IEnglandDestinationsRepository
    {
        Task<EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default);
    }
}
