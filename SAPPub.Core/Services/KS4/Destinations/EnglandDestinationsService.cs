using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4.Destinations
{
    public sealed class EnglandDestinationsService : IEnglandDestinationsService
    {
        private readonly IEnglandDestinationsRepository _repo;

        public EnglandDestinationsService(IEnglandDestinationsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default)
        {
            return await _repo.GetEnglandDestinationsAsync(ct);
        }
    }
}
