using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Repositories
{
    public interface IEstablishmentRepository
    {
        Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default);
        Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default);
        Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default);
    }
}
