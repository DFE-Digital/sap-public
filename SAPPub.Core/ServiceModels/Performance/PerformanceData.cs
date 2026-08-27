using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class PerformanceData
{
    public required CodedDouble NumberOfStudents { get; init; }

    public required CodedDouble ProgressScore { get; init; }    

    public required CodedDouble ConfidenceLevelUpper { get; init; }

    public required CodedDouble ConfidenceLevelLower { get; init; }

    public required PerformanceResult Result { get; init; }
}
