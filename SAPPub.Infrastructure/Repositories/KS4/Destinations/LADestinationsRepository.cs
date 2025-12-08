using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Destinations
{
    public class LADestinationsRepository : ILADestinationsRepository
    {
        private readonly IGenericRepository<LADestinations> _LADestinationsRepository;
        private ILogger<LADestinations> _logger;

        public LADestinationsRepository(
            IGenericRepository<LADestinations> LADestinationsRepository,
            ILogger<LADestinations> logger)
        {
            _LADestinationsRepository = LADestinationsRepository;
            _logger = logger;
        }


        public IEnumerable<LADestinations> GetAllLADestinations()
        {
            return _LADestinationsRepository.ReadAll() ?? [];
        }


        public LADestinations GetLADestinations(string laCode)
        {
            return GetAllLADestinations().FirstOrDefault(x => x.Id == laCode) ?? new LADestinations();
        }
    }
}
