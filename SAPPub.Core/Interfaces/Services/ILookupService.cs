using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Services
{
    public interface ILookupService
    {
        Task<IEnumerable<Lookup>> GetAllLookupsAsync(CancellationToken ct = default);
        Task<Lookup> GetLookupAsync(string id, CancellationToken ct = default);
    }
}
