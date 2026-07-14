using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public record AdditionalMeasures
{
    public required CodedDouble PercentAchievingAtLeastOneQualification { get; init; }
    public required CodedDouble PercentEnteredForTripleScience { get; init; }
    public required CodedDouble PercentEnteredMoreThanOneForeignLanguage { get; init; }

    public required CodedDouble AverageGCSEExamEntriesPerPupil { get; init; }
    public required CodedDouble AverageAllKS4QualificationsExamEntriesPerPupil { get; init; }

    // CML TODO -  this should be int - but does it need to be a CodedInt (that we don't have)
    // i.e. could the value be "SUPP", "z" etc? - probably not
    public required CodedDouble NumberOfPupilsAtTheEndOfKS4 { get; init; }
}
