using SAPPub.Core.Enums.KS5Qualifications;

namespace SAPPub.Core.ServiceModels.Performance;

public class AdvancedLevelQualificationModel
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; init; }

    public required bool IsKS4 { get; init; }

    public required bool IsKS5 { get; init; }

    public required string SchoolName { get; init; }

    public required Level3 QualificationType { get; init; }

    public double? TotalNoOfStudentCompletedQualification { get; init; }

    public required ProgressScoreModel ProgressScore { get; init; }
}
