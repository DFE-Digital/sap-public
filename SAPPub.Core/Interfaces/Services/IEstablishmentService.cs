using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Services
{
    public interface IEstablishmentService
    {
        Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default);
        Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default);
    }
}
