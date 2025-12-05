using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private ILogger<Establishment> _logger;


        public EstablishmentService(
            IEstablishmentRepository establishmentRepository,
            ILogger<Establishment> logger)
        {
            _establishmentRepository = establishmentRepository;
            _logger = logger;
        }


        public IEnumerable<Establishment> GetAllEstablishments()
        {
            return _establishmentRepository.GetAllEstablishments();
        }


        public Establishment GetEstablishment(string urn)
        {
            return _establishmentRepository.GetEstablishment(urn);
        }
    }
}
