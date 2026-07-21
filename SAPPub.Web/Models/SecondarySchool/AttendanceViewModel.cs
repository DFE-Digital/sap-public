using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AttendanceViewModel : BaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public required DisplayField<string> LocalAuthority { get; set; }

    public required DisplayField<double> EstablishmentAttendance { get; init; }

    public required DisplayField<double> EnglandAttendance { get; init; }

    public required DisplayField<double> LocalAuthorityAttendance { get; init; }

    public required DisplayField<double> EstablishmentPersistentAbsence { get; init; }

    public required DisplayField<double> EnglandPersistentAbsence { get; init; }

    public required DisplayField<double> LocalAuthorityPersistentAbsence { get; init; }

    public required DisplayField<double> EstablishmentEnrolmentsTotal { get; init; }

    public required DisplayField<double> EstablishmentPersistentAbsenceTotal { get; init; }

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
            EstablishmentPersistentAbsence = attendanceDetails.EstablishmentPersistentAbsence.ToDisplayField(),
            EnglandPersistentAbsence = attendanceDetails.EnglandPersistentAbsence.ToDisplayField(),
            LocalAuthorityPersistentAbsence = attendanceDetails.LocalAuthorityPersistentAbsence.ToDisplayField(),
            EstablishmentEnrolmentsTotal = attendanceDetails.EstablishmentEnrolmentsTotal.ToDisplayField(),
            EstablishmentPersistentAbsenceTotal = attendanceDetails.EstablishmentPersistentAbsenceTotal.ToDisplayField(),
            IsKS2 = attendanceDetails.IsKS2,
            IsKS4 = attendanceDetails.IsKS4,
            IsKS5 = attendanceDetails.IsKS5
        };
    }
}
