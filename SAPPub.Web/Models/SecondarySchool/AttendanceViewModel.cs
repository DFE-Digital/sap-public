using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AttendanceViewModel : SecondarySchoolBaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public required DisplayField<string> LocalAuthority { get; set; }

    public required DisplayField<double> EstablishmentAttendance { get; init; }

    public required DisplayField<double> EnglandAttendance { get; init; }

    public required DisplayField<double> LocalAuthorityAttendance { get; init; }

    public static AttendanceViewModel Map(AttendanceModel attendanceDetails)
    {
        return new AttendanceViewModel
        {
            URN = attendanceDetails.Urn,
            SchoolName = attendanceDetails.SchoolName ?? string.Empty,
            SchoolWebsite = attendanceDetails.Website.ToDisplayField(),
            LocalAuthority = attendanceDetails.LocalAuthority.ToDisplayField(),
            EstablishmentAttendance = attendanceDetails.EstablishmentAttendance.ToDisplayField(),
            EnglandAttendance = attendanceDetails.EnglandAttendance.ToDisplayField(),
            LocalAuthorityAttendance = attendanceDetails.LocalAuthorityAttendance.ToDisplayField(),
        };
    }
}
