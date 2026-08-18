using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class Level3QualificationModel
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; init; }

    public required bool IsKS4 { get; init; }

    public required bool IsKS5 { get; init; }

    public required string SchoolName { get; init; }

    public required string LAName { get; init; }

    public required Level3 QualificationType { get; init; }

    public CodedDouble TotalNoOfStudentCompletedQualification { get; init; }

    public required ProgressScoreModel ProgressScore { get; init; }

    public required AverageResultModel AverageResult { get; init; }

    public AdditionalDataModel? AdditionalData { get; init; }

    public SimpleCodedDoubleTableModel? AdvancedLevelMathsQualificationData { get; init; }
}
