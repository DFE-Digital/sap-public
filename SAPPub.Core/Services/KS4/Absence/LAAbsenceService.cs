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
    public class LAAbsenceService : ILAAbsenceService
    {
        private readonly ILAAbsenceRepository _laAbsenceRepository;


        public LAAbsenceService(
            ILAAbsenceRepository laAbsenceRepository)
        {
            _laAbsenceRepository = laAbsenceRepository;
        }


        public IEnumerable<LAAbsence> GetAllLAAbsence()
        {
            return _laAbsenceRepository.GetAllLAAbsence();
        }


        public LAAbsence GetLAAbsence(string urn)
        {
            return _laAbsenceRepository.GetLAAbsence(urn) ?? new();
        }
    }
}
