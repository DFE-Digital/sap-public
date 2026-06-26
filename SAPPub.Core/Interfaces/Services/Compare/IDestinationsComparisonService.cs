using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Interfaces.Services.Compare;

public interface IDestinationsComparisonService
{
    public Task<DestinationsComparisonResultModel> GetDestinationsDetailsAsync(IEnumerable<string> urns, CancellationToken ct = default);
}
