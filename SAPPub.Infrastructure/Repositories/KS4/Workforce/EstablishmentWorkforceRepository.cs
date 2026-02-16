using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;

namespace SAPPub.Infrastructure.Repositories.KS4.Workforce
{
    public sealed class EstablishmentWorkforceRepository : IEstablishmentWorkforceRepository
    {
        private readonly IGenericRepository<EstablishmentWorkforce> _repo;

        public EstablishmentWorkforceRepository(IGenericRepository<EstablishmentWorkforce> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EstablishmentWorkforce>> GetAllEstablishmentWorkforceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<EstablishmentWorkforce> GetEstablishmentWorkforceAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new EstablishmentWorkforce();

            return await _repo.ReadAsync(urn, ct) ?? new EstablishmentWorkforce();
        }
    }
}
