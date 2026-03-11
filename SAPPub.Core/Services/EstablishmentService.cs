using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public sealed class EstablishmentService : IEstablishmentService
    {
        private readonly IEstablishmentRepository _establishmentRepository;

        public EstablishmentService(
            IEstablishmentRepository establishmentRepository)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
        }

        public Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default)
        {
            return _establishmentRepository.GetAllEstablishmentsAsync(ct);
        }

        public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
        {
            return _establishmentRepository.GetEstablishmentsAsync(page, take, ct);
        }

        public async Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new Establishment();

            var establishment = await _establishmentRepository.GetEstablishmentAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(establishment.URN))
                return new Establishment();

            return establishment;
        }
    }
}
