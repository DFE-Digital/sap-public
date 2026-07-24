using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class EnglishMathsQualificationsViewModel : BaseViewModel
{

    public EnglishMathsScoreViewModel? AverageEnglishProgress { get; set; }

    public EnglishMathsScoreViewModel? EnteredForEnglishQualification { get; set; }

    public EnglishMathsScoreViewModel? AverageMathsProgress { get; set; }

    public EnglishMathsScoreViewModel? EnteredForMathsQualification { get; set; }

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
            AverageEnglishProgress = EnglishMathsScoreViewModel.Map(model.AverageEnglishProgress),
            AverageMathsProgress = EnglishMathsScoreViewModel.Map(model.AverageMathsProgress),
            EnteredForEnglishQualification = EnglishMathsScoreViewModel.Map(model.EnteredForEnglishQualification),
            EnteredForMathsQualification = EnglishMathsScoreViewModel.Map(model.EnteredForMathsQualification)
        };
    }    
}