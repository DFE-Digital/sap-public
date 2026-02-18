using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Absence;

namespace SAPPub.Core.Services.KS4.Absence
{
    public sealed class EstablishmentAbsenceService : IEstablishmentAbsenceService
    {
        private readonly IEstablishmentAbsenceRepository _establishmentAbsenceRepository;

        public EstablishmentAbsenceService(IEstablishmentAbsenceRepository establishmentAbsenceRepository)
        {
            _establishmentAbsenceRepository = establishmentAbsenceRepository
                ?? throw new ArgumentNullException(nameof(establishmentAbsenceRepository));
        }

        public async Task<IEnumerable<EstablishmentAbsence>> GetAllEstablishmentAbsenceAsync(CancellationToken ct = default)
        {
            return await _establishmentAbsenceRepository.GetAllEstablishmentAbsenceAsync(ct);
        }

        public async Task<EstablishmentAbsence> GetEstablishmentAbsenceAsync(string urn, CancellationToken ct = default)
        {
            return await _establishmentAbsenceRepository.GetEstablishmentAbsenceAsync(urn, ct);
        }
    }
}
