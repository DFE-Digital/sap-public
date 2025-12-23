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
    public class EnglandAbsenceService : IEnglandAbsenceService
    {
        private readonly IEnglandAbsenceRepository _englandAbsenceRepository;


        public EnglandAbsenceService(
            IEnglandAbsenceRepository englandAbsenceRepository)
        {
            _englandAbsenceRepository = englandAbsenceRepository;
        }


        public EnglandAbsence GetEnglandAbsence()
        {
            return _englandAbsenceRepository.GetEnglandAbsence();
        }
    }
}
