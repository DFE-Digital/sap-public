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
    public class TESTablishmentRepository : ITESTablishmentRepository
    {
        private readonly IGenericRepository<TESTablishment> _establishmentMetadataRepository;
        private ILogger<TESTablishment> _logger;

        public TESTablishmentRepository(
            IGenericRepository<TESTablishment> establishmentMetadataRepository, 
            ILogger<TESTablishment> logger)
        {
            _establishmentMetadataRepository = establishmentMetadataRepository;
            _logger = logger;
        }


        public IEnumerable<TESTablishment> GetAllEstablishments()
        {
            return _establishmentMetadataRepository.ReadAll() ?? [];
        }


        public TESTablishment GetEstablishment(string urn)
        {
            return GetAllEstablishments().FirstOrDefault(x => x.URN == urn) ?? new TESTablishment();
        }
    }
}
