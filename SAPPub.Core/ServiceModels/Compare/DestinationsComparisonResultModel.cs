namespace SAPPub.Core.ServiceModels.Compare;

public class DestinationsComparisonResultModel
{
    public required double? EnglandPercentage { get; set; }

    public required IEnumerable<SchoolDestinationDetails> SchoolDetails { get; set; }
}
