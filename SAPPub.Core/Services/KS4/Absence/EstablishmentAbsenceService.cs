using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.KS4.Absence
{
    public class EstablishmentAbsenceService : IEstablishmentAbsenceService
    {
        private readonly IEstablishmentAbsenceRepository _establishmentAbsenceRepository;


        public EstablishmentAbsenceService(
            IEstablishmentAbsenceRepository establishmentAbsenceRepository)
        {
            _establishmentAbsenceRepository = establishmentAbsenceRepository;
        }


        public IEnumerable<EstablishmentAbsence> GetAllEstablishmentAbsence()
        {
            return _establishmentAbsenceRepository.GetAllEstablishmentAbsence();
        }


        public EstablishmentAbsence GetEstablishmentAbsence(string urn)
        {
            return _establishmentAbsenceRepository.GetEstablishmentAbsence(urn) ?? new();
        }
    }
}
