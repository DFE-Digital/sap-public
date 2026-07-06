namespace SAPPub.Core.ServiceModels.Compare;

public class AttainmentAndProgressComparisionResultsModel
{
    public required IEnumerable<SchoolAttainmentAndProgressDetails> SchoolDetails { get; init; }

    public required double? EnglandAverage { get; init; }
}
