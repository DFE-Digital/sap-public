using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.ServiceModels.KS4.Attendance;

namespace SAPPub.Core.Services.KS4.Attendance;

public sealed class AttendanceService(
    IEstablishmentService establishmentService,
    IEstablishmentAbsenceService establishmentAbsenceService,
    IEnglandAbsenceService englandAbsenceService,
    ILAAbsenceService laAbsenceService) : IAttendanceService
{
    public async Task<AttendanceModel> GetAttendenceDetailsAsync(
        string urn,
        CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishment.URN))
            return new AttendanceModel { Urn = urn, IsKS2 = false, IsKS4 = false, IsKS5 = false };

        // Now we can run the remaining calls concurrently
        var establishmentAbsence = await establishmentAbsenceService.GetEstablishmentAbsenceAsync(urn, ct);

        var laId = establishment.LAId ?? string.Empty;
        var laAbsence = await laAbsenceService.GetLAAbsenceAsync(laId, ct);

        var englandAbsence = await englandAbsenceService.GetEnglandAbsenceAsync(ct);

        return new AttendanceModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            Website = establishment.Website,
            LocalAuthority = establishment.LAName,
            EstablishmentAttendance = GetAttendenceValue(establishmentAbsence?.Abs_Tot_Est_Current_Pct),
            EnglandAttendance = GetAttendenceValue(englandAbsence?.Abs_Tot_Eng_Current_Pct),
            LocalAuthorityAttendance = GetAttendenceValue(laAbsence?.Abs_Tot_LA_Current_Pct),
            EstablishmentPersistentAbsence = GetAbsenceValue(establishmentAbsence?.Abs_Persistent_Est_Current_Pct),
            EnglandPersistentAbsence = GetAbsenceValue(englandAbsence?.Abs_Persistent_Eng_Current_Pct),
            LocalAuthorityPersistentAbsence = GetAbsenceValue(laAbsence?.Abs_Persistent_LA_Current_Pct),
            EstablishmentEnrolmentsTotal = establishmentAbsence?.Enrolments_Tot_Est_Current_Num,
            EstablishmentPersistentAbsenceTotal = establishmentAbsence?.Abs_Persistent_Est_Current_Num
        };
    }

    private static double? GetAttendenceValue(double? absenceValue)
    {
        return absenceValue.HasValue ? Math.Round(100 - absenceValue.Value, 1) : null;
    }

    private static double? GetAbsenceValue(double? absenceValue)
    {
        return absenceValue.HasValue ? Math.Round(absenceValue.Value, 1) : null;
    }
}
