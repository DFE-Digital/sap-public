using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class ProgressScoreModel
{
    public CodedDouble Score { get; init; }

    public string? BandingRating { get; init; }

    public CodedDouble ConfidenceLevelUpper { get; init; }

    public CodedDouble ConfidenceLevelLower { get; init; }

    public CodedDouble EnglandAverageScore { get; init; }
}
