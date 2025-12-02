using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services
{
    public class EstablishmentMetadataService : IEstablishmentMetadataService
    {
        private readonly IEstablishmentMetadataRepository _establishmentMetadataRepository;
        private ILogger<EstablishmentMetadata> _logger;


        public EstablishmentMetadataService(
            IEstablishmentMetadataRepository establishmentMetadataRepository,
            ILogger<EstablishmentMetadata> logger)
        {
            _establishmentMetadataRepository = establishmentMetadataRepository;
            _logger = logger;
        }


        public IEnumerable<EstablishmentMetadata> GetAllEstablishmentMetadata()
        {
            return _establishmentMetadataRepository.GetAllEstablishmentMetadata();
        }


        public EstablishmentMetadata GetEstablishmentMetadata(string urn)
        {
            return _establishmentMetadataRepository.GetEstablishmentMetadata(urn);
        }
    }
}
