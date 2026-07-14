using SAPPub.Core.ServiceModels;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class CurriculumAndExtraCurricularActivitiesViewModel : SecondarySchoolBaseViewModel
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
