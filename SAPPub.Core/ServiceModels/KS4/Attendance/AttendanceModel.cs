using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Attendance;

public record AttendanceModel
{
    public required string Urn { get; init; }

    public string? SchoolName { get; init; }
    public required bool IsKS2 { get; set; }
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; }

    public string? Website { get; init; }

    public string? LocalAuthority { get; set; }

    public CodedDouble EstablishmentAttendance { get; init; }

    public CodedDouble EnglandAttendance { get; init; }

    public CodedDouble LocalAuthorityAttendance { get; init; }

    public CodedDouble EstablishmentPersistentAbsence { get; init; }

    public CodedDouble EnglandPersistentAbsence { get; init; }

    public CodedDouble LocalAuthorityPersistentAbsence { get; init; }

    public CodedDouble EstablishmentEnrolmentsTotal { get; init; }

    public CodedDouble EstablishmentPersistentAbsenceTotal { get; init; }
}
