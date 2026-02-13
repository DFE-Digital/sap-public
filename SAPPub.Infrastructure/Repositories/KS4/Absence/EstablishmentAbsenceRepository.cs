using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Absence
{
    public sealed class EstablishmentAbsenceRepository : IEstablishmentAbsenceRepository
    {
        private readonly IGenericRepository<EstablishmentAbsence> _repo;

        public EstablishmentAbsenceRepository(IGenericRepository<EstablishmentAbsence> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EstablishmentAbsence>> GetAllEstablishmentAbsenceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<EstablishmentAbsence> GetEstablishmentAbsenceAsync(string urn, CancellationToken ct = default)
        {
            return await _repo.ReadAsync(urn, ct) ?? new EstablishmentAbsence();
        }
    }
}
