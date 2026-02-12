using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public class LADestinationsRepository : ILADestinationsRepository
    {
        private readonly IGenericRepository<LADestinations> _repo;
        private readonly ILogger<LADestinationsRepository> _logger;

        public LADestinationsRepository(
            IGenericRepository<LADestinations> repo,
            ILogger<LADestinationsRepository> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public IEnumerable<LADestinations> GetAllLADestinations()
        {
            return _repo.ReadAll() ?? [];
        }

        public LADestinations GetLADestinations(string laCode)
        {
            // Use DB filter instead of loading everything
            return _repo.Read(laCode) ?? new LADestinations();
        }
    }
}
