using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Overview;

public sealed class OverviewModel
{
    public required string Urn { get; init; }
    public required string SchoolName { get; init; }

    public string PhaseOfEducation { get; init; } = string.Empty;
    public string AgeRangeLow { get; init; } = string.Empty;
    public string AgeRangeHigh { get; init; } = string.Empty;
    public string NumberOfPupils { get; init; } = string.Empty;
    public string? SenProvision { get; init; }
    public string Phone { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public string Easting { get; init; } = string.Empty;
    public string Northing { get; init; } = string.Empty;

    public bool IsKS2 { get; init; }
    public bool IsKS4 { get; init; }
    public bool IsKS5 { get; init; }

    public CodedDouble? Attainment8 { get; init; }

    public CodedDouble? EnglishAndMathsGrade5Establishment { get; init; }
    public CodedDouble? EnglishAndMathsGrade5LA { get; init; }
    public CodedDouble? EnglishAndMathsGrade5England { get; init; }
    public CodedDouble? MoreThanOneForeignLanguage { get; init; }

    public CodedDouble? DestinationsEstablishment { get; init; }
    public CodedDouble? DestinationsLA { get; init; }
    public CodedDouble? DestinationsEngland { get; init; }
    public CodedDouble? ReadingWritingMathsExpectedEstablishment { get; init; }
    public CodedDouble? ReadingWritingMathsExpectedLA { get; init; }
    public CodedDouble? ReadingWritingMathsExpectedEngland { get; init; }

    public CodedDouble? ReadingWritingMathsHigherEstablishment { get; init; }
    public CodedDouble? ReadingWritingMathsHigherLA { get; init; }
    public CodedDouble? ReadingWritingMathsHigherEngland { get; init; }
}