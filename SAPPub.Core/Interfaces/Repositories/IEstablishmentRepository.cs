using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Core.Interfaces.Repositories
{
    public interface IEstablishmentRepository
    {
        Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default);
        Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default);
        Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchAsync(SearchQuery query, int maxResults = 10, CancellationToken ct = default);

    }
}
