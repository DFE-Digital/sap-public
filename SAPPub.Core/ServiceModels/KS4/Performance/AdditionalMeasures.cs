using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class AdditionalMeasures
{
    public required CodedDouble AchievingAtLeastOneQualification { get; init; }
    public required CodedDouble EnteredForTripleScience { get; init; }
    public required CodedDouble EnteredMoreThanOneForeignLanguage { get; init; }
    public required CodedDouble GCSEExamEntriesPerPupil { get; init; }
    public required CodedDouble AllKS4QualificationsExamEntriesPerPupil { get; init; }
    public required CodedDouble PupilsAtTheEndOfKS4 { get; init; }
}
