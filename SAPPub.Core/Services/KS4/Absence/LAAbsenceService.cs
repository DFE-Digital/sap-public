using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Absence;

namespace SAPPub.Core.Services.KS4.Absence
{
    public sealed class LAAbsenceService : ILAAbsenceService
    {
        private readonly ILAAbsenceRepository _laAbsenceRepository;

        public LAAbsenceService(ILAAbsenceRepository laAbsenceRepository)
        {
            _laAbsenceRepository = laAbsenceRepository
                ?? throw new ArgumentNullException(nameof(laAbsenceRepository));
        }

        public async Task<IEnumerable<LAAbsence>> GetAllLAAbsenceAsync(CancellationToken ct = default)
        {
            return await _laAbsenceRepository.GetAllLAAbsenceAsync(ct);
        }

        public async Task<LAAbsence> GetLAAbsenceAsync(string id, CancellationToken ct = default)
        {
            return await _laAbsenceRepository.GetLAAbsenceAsync(id, ct);
        }
    }
}
