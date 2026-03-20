using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AttendanceViewModel : SecondarySchoolBaseViewModel
{
    public string? SchoolWebsite { get; set; }
    public string FormattedSchoolWebsite => TextHelpers.EnsureHttpsUrl(SchoolWebsite);

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
