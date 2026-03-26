using SAPPub.Core.Entities;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class CurriculumAndExtraCurricularActivitiesViewModel : SecondarySchoolBaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; set; }

    public static CurriculumAndExtraCurricularActivitiesViewModel Map(Establishment establishment)
    {
        return new CurriculumAndExtraCurricularActivitiesViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SchoolWebsite = establishment.Website.ToDisplayField()
        };
    }
}
