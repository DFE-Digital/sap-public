using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class CurriculumAndExtraCurricularActivitiesViewModel : SecondarySchoolBaseViewModel
{
    public static CurriculumAndExtraCurricularActivitiesViewModel Map(Establishment establishment)
    {
        return new CurriculumAndExtraCurricularActivitiesViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SchoolWebsite = establishment.Website
        };
    }
}
