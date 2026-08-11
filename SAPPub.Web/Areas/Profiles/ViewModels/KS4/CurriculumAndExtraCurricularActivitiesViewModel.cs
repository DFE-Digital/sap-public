using SAPPub.Core.ServiceModels;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS4;

public class CurriculumAndExtraCurricularActivitiesViewModel : BaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; set; }
    public bool IsPrimaryFeatureEnabled { get; set; }

    // Computed property - show sub-navigation only when school has both KS2 & KS4 AND primary feature is enabled
    public bool ShowSubNavigation => IsKS2 && IsKS4 && IsPrimaryFeatureEnabled;

    public static CurriculumAndExtraCurricularActivitiesViewModel Map(EstablishmentServiceModel establishment, bool isPrimaryFeatureEnabled)
    {
        return new CurriculumAndExtraCurricularActivitiesViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SchoolWebsite = establishment.Website.ToDisplayField(),
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            IsPrimaryFeatureEnabled = isPrimaryFeatureEnabled
        };
    }
}
