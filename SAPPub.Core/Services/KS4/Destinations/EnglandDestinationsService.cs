using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.KS4.Destinations
{
    public class EnglandDestinationsService : IEnglandDestinationsService
    {
        private readonly IEnglandDestinationsRepository _EnglandDestinationsRepository;

        public EnglandDestinationsService(IEnglandDestinationsRepository EnglandDestinationsRepository)
        {
            _EnglandDestinationsRepository = EnglandDestinationsRepository;
        }


        public EnglandDestinations GetEnglandDestinations()
        {
            return _EnglandDestinationsRepository.GetEnglandDestinations();
        }
    }
}
