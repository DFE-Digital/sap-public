namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISearchVisibilityPolicy
{
    Task<bool> IncludeKs5Async(CancellationToken ct = default);
}