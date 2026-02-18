using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Absence;

namespace SAPPub.Core.Services.KS4.Absence
{
    public sealed class EnglandAbsenceService : IEnglandAbsenceService
    {
        private readonly IEnglandAbsenceRepository _englandAbsenceRepository;

        public EnglandAbsenceService(IEnglandAbsenceRepository englandAbsenceRepository)
        {
            _englandAbsenceRepository = englandAbsenceRepository ?? throw new ArgumentNullException(nameof(englandAbsenceRepository));
        }

        public async Task<EnglandAbsence> GetEnglandAbsenceAsync(CancellationToken ct = default)
        {
            return await _englandAbsenceRepository.GetEnglandAbsenceAsync(ct);
        }
    }
}
