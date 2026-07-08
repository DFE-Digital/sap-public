namespace SAPPub.Core.ServiceModels.Compare;

public class AttainmentAndProgressComparisonResultsModel
{
    public required IEnumerable<SchoolAttainmentAndProgressDetails> SchoolDetails { get; init; }

    public required double? EnglandAverage { get; init; }
}
