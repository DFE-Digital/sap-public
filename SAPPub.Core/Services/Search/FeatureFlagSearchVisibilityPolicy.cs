using Microsoft.FeatureManagement;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.Specifications;

namespace SAPPub.Core.Services.Search;

public sealed class FeatureFlagSearchVisibilityPolicy(IFeatureManager featureManager) : ISearchVisibilityPolicy
{
    private const string Enable16to19 = "Enable16to19";
    private const string EnablePrimary = "EnablePrimary";

    public Task<bool> IncludeKs5Async(CancellationToken ct = default)
        => featureManager.IsEnabledAsync(Enable16to19);

    public Task<bool> IncludeKs2Async(CancellationToken ct = default)
        => featureManager.IsEnabledAsync(EnablePrimary);

    public async Task<SearchVisibilitySpecification> GetVisibilitySpecificationAsync(CancellationToken ct = default)
    {
        var includeKs5 = await IncludeKs5Async(ct);
        var includeKs2 = await IncludeKs2Async(ct);
        return new SearchVisibilitySpecification(includeKs5, includeKs2);
    }
}