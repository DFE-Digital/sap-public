using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public sealed class LADestinationsRepository : ILADestinationsRepository
    {
        private readonly IGenericRepository<LADestinations> _repo;

        public LADestinationsRepository(IGenericRepository<LADestinations> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<LADestinations>> GetAllLADestinationsAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(laCode))
                return new LADestinations();

            return await _repo.ReadAsync(laCode, ct) ?? new LADestinations();
        }
    }
}
