using SAPPub.Core.ServiceModels;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class CurriculumAndExtraCurricularActivitiesViewModel : BaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; set; }

    public static CurriculumAndExtraCurricularActivitiesViewModel Map(EstablishmentServiceModel establishment)
    {
        return new CurriculumAndExtraCurricularActivitiesViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SchoolWebsite = establishment.Website.ToDisplayField(),
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5
        };
    }
}
