using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance;

public interface IAttainmentAndProgressComparisionService
{
    Task<AttainmentAndProgressComparisonResultsModel> GetComparisionResultsAsync(IEnumerable<string> urns, CancellationToken ct = default);
}
