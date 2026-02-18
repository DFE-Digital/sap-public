using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Repositories
{
    public interface ILookupRepository
    {
        Task<IEnumerable<Lookup>> GetAllLookupsAsync(CancellationToken ct = default);
        Task<Lookup?> GetLookupAsync(string id, CancellationToken ct = default);
    }
}
