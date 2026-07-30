using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class EnglishMathsQualificationsViewModel : BaseViewModel
{
    public string? LAName { get; set; }

    public required EnglishMathsScoreViewModel AverageEnglishProgress { get; set; }

    public required EnglishMathsScoreViewModel EnteredForEnglishQualification { get; set; }

    public required EnglishMathsScoreViewModel AverageMathsProgress { get; set; }

    public required EnglishMathsScoreViewModel EnteredForMathsQualification { get; set; }

    public required EnglishMathsQualificationsDisadvantagedViewModel NumberOfDisadvantagedStudentsEnglish { get; init; }
    public required EnglishMathsQualificationsDisadvantagedViewModel ProgressOfDisadvantagedStudentsEnglish { get; init; }

    public required EnglishMathsQualificationsDisadvantagedViewModel NumberOfDisadvantagedStudentsMaths { get; init; }
    public required EnglishMathsQualificationsDisadvantagedViewModel ProgressOfDisadvantagedStudentsMaths { get; init; }

    public required EnglishMathsQualificationsDisadvantagedViewModel NumberOfNonDisadvantagedStudentsEnglish { get; init; }
    public required EnglishMathsQualificationsDisadvantagedViewModel ProgressOfNonDisadvantagedStudentsEnglish { get; init; }

    public required EnglishMathsQualificationsDisadvantagedViewModel NumberOfNonDisadvantagedStudentsMaths { get; init; }
    public required EnglishMathsQualificationsDisadvantagedViewModel ProgressOfNonDisadvantagedStudentsMaths { get; init; }


    public static EnglishMathsQualificationsViewModel Map(EnglishMathsQualificationModel model)
    {
        return new EnglishMathsQualificationsViewModel
        {
            URN = model.Urn,
            SchoolName = model.SchoolName ?? string.Empty,
            IsKS2 = model.IsKS2,
            IsKS4 = model.IsKS4,
            IsKS5 = model.IsKS5,
            LAName = model.LAName,
            AverageEnglishProgress = EnglishMathsScoreViewModel.Map(model.AverageEnglishProgress),
            AverageMathsProgress = EnglishMathsScoreViewModel.Map(model.AverageMathsProgress),
            EnteredForEnglishQualification = EnglishMathsScoreViewModel.Map(model.EnteredForEnglishQualification),
            EnteredForMathsQualification = EnglishMathsScoreViewModel.Map(model.EnteredForMathsQualification),
            NumberOfDisadvantagedStudentsEnglish = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.NumberOfDisadvantagedStudentsEnglish),
            ProgressOfDisadvantagedStudentsEnglish = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.ProgressOfDisadvantagedStudentsEnglish),
            NumberOfDisadvantagedStudentsMaths = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.NumberOfDisadvantagedStudentsMaths),
            ProgressOfDisadvantagedStudentsMaths = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.ProgressOfDisadvantagedStudentsMaths),
            NumberOfNonDisadvantagedStudentsEnglish = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.NumberOfNonDisadvantagedStudentsEnglish),
            ProgressOfNonDisadvantagedStudentsEnglish = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.ProgressOfNonDisadvantagedStudentsEnglish),
            NumberOfNonDisadvantagedStudentsMaths = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.NumberOfNonDisadvantagedStudentsMaths),
            ProgressOfNonDisadvantagedStudentsMaths = EnglishMathsQualificationsDisadvantagedViewModel.Map(model.ProgressOfNonDisadvantagedStudentsMaths),
        };
    }    
}