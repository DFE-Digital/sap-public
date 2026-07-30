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

    public required EnglishMathsQualificationsDisadvantagedModel NumberOfDisadvantagedStudentsEnglish { get; init; }
    public required EnglishMathsQualificationsDisadvantagedModel ProgressOfDisadvantagedStudentsEnglish { get; init; }

    public required EnglishMathsQualificationsDisadvantagedModel NumberOfDisadvantagedStudentsMaths { get; init; }
    public required EnglishMathsQualificationsDisadvantagedModel ProgressOfDisadvantagedStudentsMaths { get; init; }

    public required EnglishMathsQualificationsDisadvantagedModel NumberOfNonDisadvantagedStudentsEnglish { get; init; }
    public required EnglishMathsQualificationsDisadvantagedModel ProgressOfNonDisadvantagedStudentsEnglish { get; init; }

    public required EnglishMathsQualificationsDisadvantagedModel NumberOfNonDisadvantagedStudentsMaths { get; init; }
    public required EnglishMathsQualificationsDisadvantagedModel ProgressOfNonDisadvantagedStudentsMaths { get; init; }

}
