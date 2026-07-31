using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class Level3QualificationViewModel : BaseViewModel
{
    public Level3 Level3Qualification { get; set; }

    public Level2 Level2Qualification { get; set; }

    public string LevelPageTitle => GetPageTitle(Level3Qualification, Level2Qualification);

    public required string LAName { get; init; }

    public required DisplayField<CodedDouble> TotalNoOfStudentCompletedQualification { get; init; }

    public required ProgressScoreViewModel ProgressScore { get; set; }

    public required AverageResultViewModel AverageResult { get; set; }

    public static Level3QualificationViewModel Map(Level3QualificationModel model)
    {
        return new Level3QualificationViewModel
        {
            URN = model.Urn,
            SchoolName = model.SchoolName ?? string.Empty,
            LAName = CommonHelper.GetLocalAuthorityDisplayName(model.LAName),
            IsKS2 = model.IsKS2,
            IsKS4 = model.IsKS4,
            IsKS5 = model.IsKS5,
            Level3Qualification = model.QualificationType,
            TotalNoOfStudentCompletedQualification = model.TotalNoOfStudentCompletedQualification.ToDisplayField(),
            ProgressScore = ProgressScoreViewModel.Map(model.ProgressScore),
            AverageResult = AverageResultViewModel.Map(model.AverageResult)
        };
    }    
}
