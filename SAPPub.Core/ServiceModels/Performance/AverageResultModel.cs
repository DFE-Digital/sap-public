using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class AverageResultModel
{
    public required RelativeYearValues<CodedDouble> NumberOfStudents { get; init; }

    public required RelativeYearValues<PerformanceResult> Establishment { get; init; }

    public required RelativeYearValues<PerformanceResult> LocalAuthority { get; init; }

    public required RelativeYearValues<PerformanceResult> England { get; init; }
}
