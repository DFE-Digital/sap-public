using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Absence
{
    public class EnglandAbsenceRepository: IEnglandAbsenceRepository
    {
        private readonly IGenericRepository<EnglandAbsence> _englandAbsenceRepository;
        private ILogger<EnglandAbsence> _logger;

        public EnglandAbsenceRepository(
            IGenericRepository<EnglandAbsence> englandAbsenceRepository,
            ILogger<EnglandAbsence> logger)
        {
            _englandAbsenceRepository = englandAbsenceRepository;
            _logger = logger;
        }


        public EnglandAbsence GetEnglandAbsence()
        {
            return _englandAbsenceRepository.ReadAll()?.FirstOrDefault() ?? new EnglandAbsence();
        }
    }
}
