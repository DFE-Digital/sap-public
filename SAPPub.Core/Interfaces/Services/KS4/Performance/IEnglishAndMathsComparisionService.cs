using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance;

public interface IEnglishAndMathsComparisionService
{
    Task<EnglishAndMathsComparisionResultsModel> GetComparisionResultsAsync(IEnumerable<string> urns, CancellationToken ct = default);
}
