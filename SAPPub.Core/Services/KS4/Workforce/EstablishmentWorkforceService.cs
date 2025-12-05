using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.KS4.Workforce
{
    public class EstablishmentWorkforceService : IEstablishmentWorkforceService
    {
        private readonly IEstablishmentWorkforceRepository _establishmentWorkforceRepository;
        private ILogger<EstablishmentWorkforce> _logger;


        public EstablishmentWorkforceService(
            IEstablishmentWorkforceRepository establishmentWorkforceRepository,
            ILogger<EstablishmentWorkforce> logger)
        {
            _establishmentWorkforceRepository = establishmentWorkforceRepository;
            _logger = logger;
        }


        public IEnumerable<EstablishmentWorkforce> GetAllEstablishmentWorkforce()
        {
            return _establishmentWorkforceRepository.GetAllEstablishmentWorkforce();
        }


        public EstablishmentWorkforce GetEstablishmentWorkforce(string urn)
        {
            return _establishmentWorkforceRepository.GetEstablishmentWorkforce(urn);
        }
    }
}
