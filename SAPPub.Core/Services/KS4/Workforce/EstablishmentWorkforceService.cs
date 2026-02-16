using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;

namespace SAPPub.Core.Services.KS4.Workforce
{
    public sealed class EstablishmentWorkforceService : IEstablishmentWorkforceService
    {
        private readonly IEstablishmentWorkforceRepository _repo;

        public EstablishmentWorkforceService(IEstablishmentWorkforceRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<EstablishmentWorkforce>> GetAllEstablishmentWorkforceAsync(CancellationToken ct = default)
        {
            return await _repo.GetAllEstablishmentWorkforceAsync(ct);
        }

        public async Task<EstablishmentWorkforce> GetEstablishmentWorkforceAsync(string urn, CancellationToken ct = default)
        {
            return await _repo.GetEstablishmentWorkforceAsync(urn, ct);
        }
    }
}
