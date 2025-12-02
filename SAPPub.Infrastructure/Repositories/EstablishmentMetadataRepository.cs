using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories
{
    public class EstablishmentMetadataRepository : IEstablishmentMetadataRepository
    {
        private readonly IGenericRepository<EstablishmentMetadata> _establishmentMetadataRepository;
        private ILogger<EstablishmentMetadata> _logger;

        public EstablishmentMetadataRepository(
            IGenericRepository<EstablishmentMetadata> establishmentMetadataRepository, 
            ILogger<EstablishmentMetadata> logger)
        {
            _establishmentMetadataRepository = establishmentMetadataRepository;
            _logger = logger;
        }


        public IEnumerable<EstablishmentMetadata> GetAllEstablishmentMetadata()
        {
            return _establishmentMetadataRepository.ReadAll() ?? [];
        }


        public EstablishmentMetadata GetEstablishmentMetadata(string id)
        {
            return GetAllEstablishmentMetadata().First(x => x.Id == id) ?? new EstablishmentMetadata();
        }
    }
}
