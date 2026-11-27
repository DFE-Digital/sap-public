using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Overview;

public sealed class OverviewModel
{
    public required string Urn { get; init; }
    public required string SchoolName { get; init; }
    public string LocalAuthorityName { get; init; } = string.Empty;

    public DateOnly? ClosedDate { get; set; }
    public DateOnly? OpenDate { get; set; }
    public EstablishmentStatus? Status { get; set; }
    public int? OpenReasonId { get; set; }

    public string PhaseOfEducation { get; init; } = string.Empty;
    public string AgeRangeLow { get; init; } = string.Empty;
    public string AgeRangeHigh { get; init; } = string.Empty;
    public string NumberOfPupils { get; init; } = string.Empty;
    public string? SenProvision { get; init; }
    public string Phone { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Easting { get; init; } = string.Empty;
    public string Northing { get; init; } = string.Empty;

    public bool IsKS2 { get; init; }
    public bool IsKS4 { get; init; }
    public bool IsKS5 { get; init; }

    public CodedDouble? Attainment8 { get; init; }

    public CodedDouble? Attainment8LA { get; init; }

    public CodedDouble? Attainment8England { get; init; }

    public CodedDouble? EnglishAndMathsGrade5Establishment { get; init; }
    public CodedDouble? EnglishAndMathsGrade5LA { get; init; }
    public CodedDouble? EnglishAndMathsGrade5England { get; init; }
    public CodedDouble? MoreThanOneForeignLanguage { get; init; }
    public CodedDouble? GcseSubjectsEntered { get; init; }
    public CodedDouble? TechnicalSubjectsEntered { get; init; }
    public IReadOnlyList<TechnicalSubjectModel> TopTechnicalSubjects { get; init; } = Array.Empty<TechnicalSubjectModel>();

    public CodedDouble? DestinationsEstablishment { get; init; }
    public CodedDouble? DestinationsLA { get; init; }
    public CodedDouble? DestinationsEngland { get; init; }
    public CodedDouble? ReadingWritingMathsExpectedEstablishment { get; init; }
    public CodedDouble? ReadingWritingMathsExpectedLA { get; init; }
    public CodedDouble? ReadingWritingMathsExpectedEngland { get; init; }

    public CodedDouble? ReadingWritingMathsHigherEstablishment { get; init; }
    public CodedDouble? ReadingWritingMathsHigherLA { get; init; }
    public CodedDouble? ReadingWritingMathsHigherEngland { get; init; }

    public CodedString? ReadingProgressBanding { get; init; }
    public CodedString? WritingProgressBanding { get; init; }
    public CodedString? MathsProgressBanding { get; init; }

    public CodedString? ReadingProgressSignificance { get; init; }
    public CodedString? WritingProgressSignificance { get; init; }
    public CodedString? MathsProgressSignificance { get; init; }

    public IList<EstablishmentLinkModel>? Predecessors { get; set; }
    public IList<EstablishmentLinkModel>? Successors { get; set; }
}