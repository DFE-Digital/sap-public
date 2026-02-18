using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4.Destinations
{
    public sealed class LADestinationsService : ILADestinationsService
    {
        private readonly ILADestinationsRepository _repo;

        public LADestinationsService(ILADestinationsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<LADestinations>> GetAllLADestinationsAsync(CancellationToken ct = default)
        {
            return await _repo.GetAllLADestinationsAsync(ct);
        }

        public async Task<LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default)
        {
            return await _repo.GetLADestinationsAsync(laCode, ct);
        }
    }
}
