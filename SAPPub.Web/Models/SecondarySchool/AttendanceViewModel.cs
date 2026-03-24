using SAPPub.Core.Entities;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AttendanceViewModel : SecondarySchoolBaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public static AttendanceViewModel Map(Establishment establishment)
    {
        return new AttendanceViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SchoolWebsite = establishment.Website.ToDisplayField()
        };
    }
}
