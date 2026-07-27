namespace SAPPub.Core.ServiceModels.Performance;

public class EnglishMathsQualificationModel
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; init; }

    public required bool IsKS4 { get; init; }

    public required bool IsKS5 { get; init; }

    public required string SchoolName { get; init; }

    public string? LAName { get; set; }

    public required EnglishMathsScoreModel AverageEnglishProgress { get; init; }

    public required EnglishMathsScoreModel EnteredForEnglishQualification { get; init; }
    
    public required EnglishMathsScoreModel AverageMathsProgress { get; init; }
    
    public required EnglishMathsScoreModel EnteredForMathsQualification { get; init; }
}
