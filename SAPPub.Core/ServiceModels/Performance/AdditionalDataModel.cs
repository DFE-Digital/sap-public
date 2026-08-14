
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class AdditionalDataModel
{
    public required CodedDouble TotalNoOfStudentsIncludedInThisMeasure { get; init; }

    public required PerformanceResult Establishment { get; init; }

    public required PerformanceResult LocalAuthority { get; init; }

    public required PerformanceResult England { get; init; }
}
