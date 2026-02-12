using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public sealed class EnglandDestinationsRepository : IEnglandDestinationsRepository
    {
        private readonly IGenericRepository<EnglandDestinations> _repo;
        private readonly ILogger<EnglandDestinationsRepository> _logger;

        public EnglandDestinationsRepository(
            IGenericRepository<EnglandDestinations> repo,
            ILogger<EnglandDestinationsRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public EnglandDestinations GetEnglandDestinations()
        {
            // v_england_destinations returns a single row
            return _repo.ReadSingle(new { }) ?? new EnglandDestinations();
        }
    }
}
