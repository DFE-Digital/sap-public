using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.SecondarySchool;

public class CurriculumAndExtraCurricularActivitiesViewModel : SecondarySchoolBaseViewModel
{
    public string? SchoolWebsite { get; set; }

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
