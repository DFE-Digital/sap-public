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
            return new AttendanceModel { Urn = urn };

        // Now we can run the remaining calls concurrently
        var establishmentAbsence = await establishmentAbsenceService.GetEstablishmentAbsenceAsync(urn, ct);

        var laId = establishment.LAId ?? string.Empty;
        var laAbsence = await laAbsenceService.GetLAAbsenceAsync(laId, ct);

        var englandAbsence = await englandAbsenceService.GetEnglandAbsenceAsync(ct);

        return new AttendanceModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            Website = establishment.Website,
            LocalAuthority = establishment.LAName,
            EstablishmentAttendance = GetAttendenceValue(establishmentAbsence?.Abs_Tot_Est_Current_Pct),
            EnglandAttendance = GetAttendenceValue(englandAbsence?.Abs_Tot_Eng_Current_Pct),
            LocalAuthorityAttendance = GetAttendenceValue(laAbsence?.Abs_Tot_LA_Current_Pct)
        };
    }

    private static double? GetAttendenceValue(double? absenceValue)
    {
        return absenceValue.HasValue ? Math.Round(100 - absenceValue.Value, 1) : null;
    }
}
