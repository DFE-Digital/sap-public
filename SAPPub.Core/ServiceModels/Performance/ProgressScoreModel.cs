namespace SAPPub.Core.ServiceModels.Performance;

public class ProgressScoreModel
{
    public double? Score { get; init; }

    public string? BandingRating { get; init; }

    public double? ConfidenceLevelUpper { get; init; }

    public double? ConfidenceLevelLower { get; init; }

    public double? EnglandAverageScore { get; init; }
}
