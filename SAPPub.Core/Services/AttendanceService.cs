using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services;

public sealed class AttendanceService(
    IEstablishmentService establishmentService,
    IEstablishmentAbsenceService establishmentAbsenceService,
    IEnglandAbsenceService englandAbsenceService,
    ILAAbsenceService laAbsenceService) : IAttendanceService
{
    public async Task<AttendanceModel> GetAttendenceDetailsAsync(string urn, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var est = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(est.URN))
        {
            return new AttendanceModel { Urn = urn, IsKS2 = false, IsKS4 = false, IsKS5 = false };
        }

        var establishmentAbsenceTask = establishmentAbsenceService.GetEstablishmentAbsenceAsync(urn, ct);
        var laAbsenceTask = laAbsenceService.GetLAAbsenceAsync(est.LAId ?? string.Empty, ct);
        var englandAbsenceTask = englandAbsenceService.GetEnglandAbsenceAsync(ct);

        await Task.WhenAll(establishmentAbsenceTask, laAbsenceTask, englandAbsenceTask);

        var estAbsence = await establishmentAbsenceTask;
        var laAbsence = await laAbsenceTask;
        var engAbsence = await englandAbsenceTask;

        return  new AttendanceModel
        {
            Urn = est.URN,
            SchoolName = est.EstablishmentName,
            IsKS2 = est.IsKS2,
            IsKS4 = est.IsKS4,
            IsKS5 = est.IsKS5,
            Website = est.Website,
            LocalAuthority = est.LAName,
            EstablishmentAttendance = GetAttendanceValue(est.IsSpecialSchool
                ? estAbsence.Abs_PersistentSPE_Est_Current_Pct_Coded
                : est.IsKS4 ? estAbsence.Abs_Tot_Est_Current_Pct_Coded : estAbsence.Abs_TotKS2_Est_Current_Pct_Coded),
            EstablishmentEnrolmentsTotal = estAbsence.Enrolments_Tot_Est_Current_Num_Coded,
            EstablishmentPersistentAbsence = est.IsSpecialSchool
                ? estAbsence.Abs_PersistentSPE_Est_Current_Pct_Coded
                : est.IsKS4 ? estAbsence.Abs_Persistent_Est_Current_Pct_Coded : estAbsence.Abs_PersistentKS2_Est_Current_Pct_Coded,
            EstablishmentPersistentAbsenceTotal = est.IsSpecialSchool
                ? estAbsence.Abs_PersistentSPE_Est_Current_Num_Coded
                : est.IsKS4 ? estAbsence.Abs_Persistent_Est_Current_Num_Coded : estAbsence.Abs_PersistentKS2_Est_Current_Num_Coded,
            EnglandAttendance = GetAttendanceValue(est.IsSpecialSchool
                ? engAbsence.Abs_TotSPE_Eng_Current_Pct_Coded 
                : est.IsKS4 ? engAbsence.Abs_Tot_Eng_Current_Pct_Coded : engAbsence.Abs_TotKS2_Eng_Current_Pct_Coded),
            EnglandPersistentAbsence = est.IsSpecialSchool ? 
                engAbsence.Abs_PersistentSPE_Eng_Current_Pct_Coded
                : est.IsKS4 ? engAbsence.Abs_Persistent_Eng_Current_Pct_Coded : engAbsence.Abs_PersistentKS2_Eng_Current_Pct_Coded,
            LocalAuthorityAttendance = GetAttendanceValue(est.IsSpecialSchool ?
                laAbsence.Abs_TotSPE_LA_Current_Pct_Coded
                : est.IsKS4 ? laAbsence.Abs_Tot_LA_Current_Pct_Coded : laAbsence.Abs_TotKS2_LA_Current_Pct_Coded),
            LocalAuthorityPersistentAbsence = est.IsSpecialSchool ?
                laAbsence.Abs_PersistentSPE_LA_Current_Pct_Coded
                : est.IsKS4 ? laAbsence.Abs_Persistent_LA_Current_Pct_Coded : laAbsence.Abs_PersistentKS2_LA_Current_Pct_Coded
        };
    }
    private static CodedDouble GetAttendanceValue(CodedDouble codedDouble)
    {
        return new CodedDouble(100 - codedDouble.Value, codedDouble.Reason, codedDouble.Raw);
    }
}