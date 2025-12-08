using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public class EnglandDestinationsRepository : IEnglandDestinationsRepository
    {
        private readonly IGenericRepository<EnglandDestinations> _EnglandDestinationsRepository;
        private readonly ILogger<EnglandDestinations> _logger;

        public EnglandDestinationsRepository(
            IGenericRepository<EnglandDestinations> EnglandDestinationsRepository,
            ILogger<EnglandDestinations> logger)
        {
            _EnglandDestinationsRepository = EnglandDestinationsRepository;
            _logger = logger;
        }


        public EnglandDestinations GetEnglandDestinations()
        {
            return _EnglandDestinationsRepository.ReadAll()?.FirstOrDefault() ?? new EnglandDestinations();
        }
    }
}
