using SAPPub.Core.Specifications;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISearchVisibilityPolicy
{
    Task<bool> IncludeKs5Async(CancellationToken ct = default);
    Task<bool> IncludeKs2Async(CancellationToken ct = default);
    Task<SearchVisibilitySpecification> GetVisibilitySpecificationAsync(CancellationToken ct = default);
}