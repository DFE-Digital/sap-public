using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;

namespace SAPPub.Infrastructure.Repositories.KS4.Absence
{
    public sealed class LAAbsenceRepository : ILAAbsenceRepository
    {
        private readonly IGenericRepository<LAAbsence> _repo;

        public LAAbsenceRepository(IGenericRepository<LAAbsence> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<LAAbsence>> GetAllLAAbsenceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<LAAbsence> GetLAAbsenceAsync(string id, CancellationToken ct = default)
        {
            return await _repo.ReadAsync(id, ct) ?? new LAAbsence();
        }
    }
}
