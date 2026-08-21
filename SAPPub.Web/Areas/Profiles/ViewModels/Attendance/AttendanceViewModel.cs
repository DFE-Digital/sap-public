using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Attendance;

public class AttendanceViewModel : BaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public required DisplayField<string> LocalAuthority { get; set; }

    public required DisplayField<CodedDouble> EstablishmentAttendance { get; init; }

    public required DisplayField<CodedDouble> EnglandAttendance { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityAttendance { get; init; }

    public required DisplayField<CodedDouble> EstablishmentPersistentAbsence { get; init; }

    public required DisplayField<CodedDouble> EnglandPersistentAbsence { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityPersistentAbsence { get; init; }

    public required DisplayField<CodedDouble> EstablishmentEnrolmentsTotal { get; init; }

    public required DisplayField<CodedDouble> EstablishmentPersistentAbsenceTotal { get; init; }

    public static AttendanceViewModel Map(AttendanceModel attendanceDetails)
    {
        return new AttendanceViewModel
        {
            URN = attendanceDetails.Urn,
            SchoolName = attendanceDetails.SchoolName ?? string.Empty,
            SchoolWebsite = attendanceDetails.Website.ToDisplayField(),
            LocalAuthority = attendanceDetails.LocalAuthority.ToDisplayField(),
            EstablishmentAttendance = attendanceDetails.EstablishmentAttendance.ToDisplayField().Round(),
            EnglandAttendance = attendanceDetails.EnglandAttendance.ToDisplayField().Round(),
            LocalAuthorityAttendance = attendanceDetails.LocalAuthorityAttendance.ToDisplayField().Round(),
            EstablishmentPersistentAbsence = attendanceDetails.EstablishmentPersistentAbsence.ToDisplayField().Round(),
            EnglandPersistentAbsence = attendanceDetails.EnglandPersistentAbsence.ToDisplayField().Round(),
            LocalAuthorityPersistentAbsence = attendanceDetails.LocalAuthorityPersistentAbsence.ToDisplayField().Round(),
            EstablishmentEnrolmentsTotal = attendanceDetails.EstablishmentEnrolmentsTotal.ToDisplayField().Round(),
            EstablishmentPersistentAbsenceTotal = attendanceDetails.EstablishmentPersistentAbsenceTotal.ToDisplayField().Round(),
            IsKS2 = attendanceDetails.IsKS2,
            IsKS4 = attendanceDetails.IsKS4,
            IsKS5 = attendanceDetails.IsKS5
        };
    }
}
