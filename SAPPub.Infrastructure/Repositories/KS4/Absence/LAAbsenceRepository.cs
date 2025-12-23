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
    public class LAAbsenceRepository: ILAAbsenceRepository
    {
        private readonly IGenericRepository<LAAbsence> _laAbsenceRepository;
        private ILogger<LAAbsence> _logger;

        public LAAbsenceRepository(
            IGenericRepository<LAAbsence> laAbsenceRepository,
            ILogger<LAAbsence> logger)
        {
            _laAbsenceRepository = laAbsenceRepository;
            _logger = logger;
        }


        public IEnumerable<LAAbsence> GetAllLAAbsence()
        {
            return _laAbsenceRepository.ReadAll() ?? [];
        }


        public LAAbsence GetLAAbsence(string urn)
        {
            return GetAllLAAbsence().FirstOrDefault(x => x.Id == urn) ?? new LAAbsence();
        }
    }
}
