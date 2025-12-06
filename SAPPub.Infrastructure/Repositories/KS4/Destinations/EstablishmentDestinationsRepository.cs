using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Destinations;
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
    public class EstablishmentDestinationsRepository : IEstablishmentDestinationsRepository
    {
        private readonly IGenericRepository<EstablishmentDestinations> _establishmentDestinationsRepository;
        private ILogger<EstablishmentDestinations> _logger;

        public EstablishmentDestinationsRepository(
            IGenericRepository<EstablishmentDestinations> establishmentDestinationsRepository,
            ILogger<EstablishmentDestinations> logger)
        {
            _establishmentDestinationsRepository = establishmentDestinationsRepository;
            _logger = logger;
        }


        public IEnumerable<EstablishmentDestinations> GetAllEstablishmentDestinations()
        {
            return _establishmentDestinationsRepository.ReadAll() ?? [];
        }


        public EstablishmentDestinations GetEstablishmentDestinations(string urn)
        {
            return GetAllEstablishmentDestinations().FirstOrDefault(x => x.Id == urn) ?? new EstablishmentDestinations();
        }
    }
}
