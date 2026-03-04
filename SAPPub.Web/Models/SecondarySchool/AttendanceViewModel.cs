using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.SecondarySchool;

public class AttendanceViewModel : SecondarySchoolBaseViewModel
{
    public required string SchoolWebsite { get; set; }

    public static AttendanceViewModel Map(Establishment establishment)
    {
        return new AttendanceViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SchoolWebsite = establishment.Website
        };
    }
}
