namespace SAPPub.Core.ServiceModels.KS4.Attendance;

public record AttendanceModel
{
    public required string Urn { get; init; }

    public string? SchoolName { get; init; }

    public string? Website { get; init; }

    public string? LocalAuthority { get; set; }

    public double? EstablishmentAttendance { get; init; }

    public double? EnglandAttendance { get; init; }

    public double? LocalAuthorityAttendance { get; init; }

    public double? EstablishmentPersistentAbsence { get; init; }

    public double? EnglandPersistentAbsence { get; init; }

    public double? LocalAuthorityPersistentAbsence { get; init; }

    public double? EstablishmentEnrolmentsTotal { get; init; }

    public double? EstablishmentPersistentAbsenceTotal { get; init; }
}
