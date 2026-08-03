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

    public required SimpleCodedDoubleTableViewModel NumberOfDisadvantagedStudentsEnglish { get; init; }
    public required SimpleCodedDoubleTableViewModel ProgressOfDisadvantagedStudentsEnglish { get; init; }

    public required SimpleCodedDoubleTableViewModel NumberOfDisadvantagedStudentsMaths { get; init; }
    public required SimpleCodedDoubleTableViewModel ProgressOfDisadvantagedStudentsMaths { get; init; }

    public required SimpleCodedDoubleTableViewModel NumberOfNonDisadvantagedStudentsEnglish { get; init; }
    public required SimpleCodedDoubleTableViewModel ProgressOfNonDisadvantagedStudentsEnglish { get; init; }

    public required SimpleCodedDoubleTableViewModel NumberOfNonDisadvantagedStudentsMaths { get; init; }
    public required SimpleCodedDoubleTableViewModel ProgressOfNonDisadvantagedStudentsMaths { get; init; }


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
            NumberOfDisadvantagedStudentsEnglish = SimpleCodedDoubleTableViewModel.Map(model.NumberOfDisadvantagedStudentsEnglish),
            ProgressOfDisadvantagedStudentsEnglish = SimpleCodedDoubleTableViewModel.Map(model.ProgressOfDisadvantagedStudentsEnglish),
            NumberOfDisadvantagedStudentsMaths = SimpleCodedDoubleTableViewModel.Map(model.NumberOfDisadvantagedStudentsMaths),
            ProgressOfDisadvantagedStudentsMaths = SimpleCodedDoubleTableViewModel.Map(model.ProgressOfDisadvantagedStudentsMaths),
            NumberOfNonDisadvantagedStudentsEnglish = SimpleCodedDoubleTableViewModel.Map(model.NumberOfNonDisadvantagedStudentsEnglish),
            ProgressOfNonDisadvantagedStudentsEnglish = SimpleCodedDoubleTableViewModel.Map(model.ProgressOfNonDisadvantagedStudentsEnglish),
            NumberOfNonDisadvantagedStudentsMaths = SimpleCodedDoubleTableViewModel.Map(model.NumberOfNonDisadvantagedStudentsMaths),
            ProgressOfNonDisadvantagedStudentsMaths = SimpleCodedDoubleTableViewModel.Map(model.ProgressOfNonDisadvantagedStudentsMaths),
        };
    }    
}