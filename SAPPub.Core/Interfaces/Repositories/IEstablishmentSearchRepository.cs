using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Core.Interfaces.Repositories
{
    public interface IEstablishmentSearchRepository
    {
        Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchAsync(SearchQuery query, int maxResults = 10, CancellationToken ct = default);
    }
}
