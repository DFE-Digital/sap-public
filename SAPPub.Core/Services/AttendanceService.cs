using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.ServiceModels;
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

        var estAbs = await establishmentAbsenceTask;
        var laAbs = await laAbsenceTask;
        var engAbs = await englandAbsenceTask;

        return new AttendanceModel
        {
            Urn = est.URN,
            SchoolName = est.EstablishmentName,
            IsKS2 = est.IsKS2,
            IsKS4 = est.IsKS4,
            IsKS5 = est.IsKS5,
            Website = est.Website,
            LocalAuthority = est.LAName,
            EstablishmentEnrolmentsTotal =
                GetCodedValue(est,
                estAbs.Enrolments_TotSPE_Est_Current_Num_Coded,
                estAbs.Enrolments_Tot_Est_Current_Num_Coded,
                estAbs.Enrolments_TotKS2_Est_Current_Num_Coded,
                false),
            EstablishmentAttendance =
                GetCodedValue(est,
                    estAbs.Abs_TotSPE_Est_Current_Pct_Coded,
                    estAbs.Abs_Tot_Est_Current_Pct_Coded,
                    estAbs.Abs_TotKS2_Est_Current_Pct_Coded,
                    true),
            EstablishmentPersistentAbsence =
                GetCodedValue(est,
                    estAbs.Abs_PersistentSPE_Est_Current_Pct_Coded, 
                    estAbs.Abs_Persistent_Est_Current_Pct_Coded, 
                    estAbs.Abs_PersistentKS2_Est_Current_Pct_Coded, 
                    false),
            EstablishmentPersistentAbsenceTotal = 
                GetCodedValue(est, 
                    estAbs.Abs_PersistentSPE_Est_Current_Num_Coded, 
                    estAbs.Abs_Persistent_Est_Current_Num_Coded, 
                    estAbs.Abs_PersistentKS2_Est_Current_Num_Coded, 
                    false),
            EnglandAttendance = 
                GetCodedValue(est, 
                    engAbs.Abs_TotSPE_Eng_Current_Pct_Coded, 
                    engAbs.Abs_Tot_Eng_Current_Pct_Coded, 
                    engAbs.Abs_TotKS2_Eng_Current_Pct_Coded, 
                    true),
            EnglandPersistentAbsence = 
                GetCodedValue(est, 
                    engAbs.Abs_PersistentSPE_Eng_Current_Pct_Coded, 
                    engAbs.Abs_Persistent_Eng_Current_Pct_Coded, 
                    engAbs.Abs_PersistentKS2_Eng_Current_Pct_Coded, 
                    false),
            LocalAuthorityAttendance = 
                GetCodedValue(est, 
                    laAbs.Abs_TotSPE_LA_Current_Pct_Coded, 
                    laAbs.Abs_Tot_LA_Current_Pct_Coded, 
                    laAbs.Abs_TotKS2_LA_Current_Pct_Coded, 
                    true),
            LocalAuthorityPersistentAbsence =
                GetCodedValue(est, 
                    laAbs.Abs_PersistentSPE_LA_Current_Pct_Coded, 
                    laAbs.Abs_Persistent_LA_Current_Pct_Coded, 
                    laAbs.Abs_PersistentKS2_LA_Current_Pct_Coded, 
                    false)
        };
    }

    /// <summary>
    /// Use to get the property value for the coded double in the model based on school type/phase.
    /// Data selection follows the following precedence order:
    /// - Special School
    /// - KS4
    /// - KS2 only school
    /// </summary>
    /// <param name="est">Establishment details</param>
    /// <param name="specSchoolVal">Database column name to map if we're taking the value for a special school</param>
    /// <param name="ks4Val">Database column name to map if we're taking the value for a KS4 phase school</param>
    /// <param name="ks2Val">Database column name to map if we're taking the value for a KS2-only phase school</param>
    /// <param name="isAttendanceVal">If true, we invert the value so we have the attendance value (rather than the absence value)</param>
    /// <returns></returns>
    private static CodedDouble GetCodedValue(EstablishmentMinimumServiceModel est, CodedDouble specSchoolVal, CodedDouble ks4Val, CodedDouble ks2Val, bool isAttendanceVal)
    {
        var retVal = est.IsSpecialSchool ? specSchoolVal : est.IsKS4 ? ks4Val : ks2Val;
        if (isAttendanceVal)
        {
            return new CodedDouble(100 - retVal.Value, retVal.Reason, retVal.Raw);
        }

        return retVal;
    }
}