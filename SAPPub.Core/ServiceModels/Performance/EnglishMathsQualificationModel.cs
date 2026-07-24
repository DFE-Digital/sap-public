namespace SAPPub.Core.ServiceModels.Performance;

public class EnglishMathsQualificationModel
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; init; }

    public required bool IsKS4 { get; init; }

    public required bool IsKS5 { get; init; }

    public required string SchoolName { get; init; }

    public string? LAName { get; set; }

    public EnglishMathsScoreModel? AverageEnglishProgress { get; set; }

    public EnglishMathsScoreModel? EnteredForEnglishQualification { get; set; }
    
    public EnglishMathsScoreModel? AverageMathsProgress { get; set; }
    
    public EnglishMathsScoreModel? EnteredForMathsQualification { get; set; }
}
