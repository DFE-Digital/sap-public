using Microsoft.FeatureManagement;
using SAPPub.Core.Interfaces.Services.Search;

namespace SAPPub.Core.Services.Search;

public sealed class FeatureFlagSearchVisibilityPolicy(IFeatureManager featureManager) : ISearchVisibilityPolicy
{
    private const string Enable16to19 = "Enable16to19";

    public Task<bool> IncludeKs5Async(CancellationToken ct = default)
        => featureManager.IsEnabledAsync(Enable16to19);
}