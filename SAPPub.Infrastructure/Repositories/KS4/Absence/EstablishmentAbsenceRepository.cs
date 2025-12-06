using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Absence
{
    public class EstablishmentAbsenceRepository: IEstablishmentAbsenceRepository
    {
        private readonly IGenericRepository<EstablishmentAbsence> _establishmentAbsenceRepository;
        private ILogger<EstablishmentAbsence> _logger;

        public EstablishmentAbsenceRepository(
            IGenericRepository<EstablishmentAbsence> establishmentAbsenceRepository,
            ILogger<EstablishmentAbsence> logger)
        {
            _establishmentAbsenceRepository = establishmentAbsenceRepository;
            _logger = logger;
        }


        public IEnumerable<EstablishmentAbsence> GetAllEstablishmentAbsence()
        {
            return _establishmentAbsenceRepository.ReadAll() ?? [];
        }


        public EstablishmentAbsence GetEstablishmentAbsence(string urn)
        {
            return GetAllEstablishmentAbsence().FirstOrDefault(x => x.Id == urn) ?? new EstablishmentAbsence();
        }
    }
}
