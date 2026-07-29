using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class EnglishMathsQualificationsViewModel : BaseViewModel
{

    public required EnglishMathsScoreViewModel AverageEnglishProgress { get; set; }

    public required EnglishMathsScoreViewModel EnteredForEnglishQualification { get; set; }

    public required EnglishMathsScoreViewModel AverageMathsProgress { get; set; }

    public required EnglishMathsScoreViewModel EnteredForMathsQualification { get; set; }

    public string? LAName { get; set; }

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
            EnteredForMathsQualification = EnglishMathsScoreViewModel.Map(model.EnteredForMathsQualification)
        };
    }    
}