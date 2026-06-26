namespace SAPPub.Core.ServiceModels.Compare;

public class SchoolDestinationDetails
{
    public required string URN { get; set; }
    public required double? PercentInEducationEmploymentOrTraining { get; set; }
}

public class DestinationsComparisonResultModel
{
    public required double? EnglandPercentage { get; set; }

    public required IEnumerable<SchoolDestinationDetails> SchoolDetails { get; set; }
}
