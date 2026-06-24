using SAPPub.Core.Entities;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class EnglishAndMathsComparisionResultModel
{
    public required string Urn { get; init; }

    public required string SchoolName { get; init; }

    public required RelativeYearValues<double?> EstablishmentData { get; init; }
}
