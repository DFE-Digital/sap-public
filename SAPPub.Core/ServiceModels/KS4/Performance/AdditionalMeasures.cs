using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public record AdditionalMeasures
{
    public required CodedDouble PercentAchievingAtLeastOneQualification { get; init; }
    public required CodedDouble PercentEnteredForTripleScience { get; init; }
    public required CodedDouble PercentEnteredMoreThanOneForeignLanguage { get; init; }

    public required CodedDouble AverageGCSEExamEntriesPerPupil { get; init; }
    public required CodedDouble AverageAllKS4QualificationsExamEntriesPerPupil { get; init; }

    public CodedDouble AverageGCSEExamEntriesPerDisadvantagedPupil { get; init; }
    public CodedDouble AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil { get; init; }
    public CodedDouble AverageGCSEExamEntriesPerNonDisadvantagedPupil { get; init; }
    public CodedDouble AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil { get; init; }

    // This should be CodedInt (that we don't have yet)
    public required CodedDouble NumberOfPupilsAtTheEndOfKS4 { get; init; }
}
