using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public sealed class EnglandDestinationsRepository : IEnglandDestinationsRepository
    {
        private readonly IGenericRepository<EnglandDestinations> _repo;

        public EnglandDestinationsRepository(IGenericRepository<EnglandDestinations> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default)
        {
            // v_england_destinations returns a single row
            return await _repo.ReadSingleAsync(new { }, ct) ?? new EnglandDestinations();
        }
    }
}
