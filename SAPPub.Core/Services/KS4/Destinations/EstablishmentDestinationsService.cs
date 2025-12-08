using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4.Destinations
{
    public class EstablishmentDestinationsService : IEstablishmentDestinationsService
    {
        private readonly IEstablishmentDestinationsRepository _establishmentDestinationsRepository;

        public EstablishmentDestinationsService(IEstablishmentDestinationsRepository establishmentDestinationsRepository)
        {
            _establishmentDestinationsRepository = establishmentDestinationsRepository;
        }


        public IEnumerable<EstablishmentDestinations> GetAllEstablishmentDestinations()
        {
            return _establishmentDestinationsRepository.GetAllEstablishmentDestinations();
        }


        public EstablishmentDestinations GetEstablishmentDestinations(string urn)
        {
            return _establishmentDestinationsRepository.GetEstablishmentDestinations(urn) ?? new(); ;
        }
    }
}
