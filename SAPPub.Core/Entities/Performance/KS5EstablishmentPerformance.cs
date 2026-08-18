using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Entities.Performance;

public class KS5EstablishmentPerformance
{
    public string Id { get; set; } = string.Empty;

    // A level Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_ALEV_1618_Est_Current_Num_Coded { get; set; }

    // A level Progress score for the school / college
    public CodedDouble VA_INS_ALEV_Est_Current_Num_Coded { get; set; }

    // A level Progress banding for the school / college
    public CodedString PROGRESS_BAND_ALEV_Est_Current { get; set; }

    // A level Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_ALEV_Est_Current_Num_Coded { get; set; }

    // A level Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_ALEV_Est_Current_Num_Coded { get; set; }

    // A level Average result points for the school / college lower
    public CodedDouble TALLPPE_ALEV_1618_Est_Current_Num_Coded { get; set; }

    // A level Average result grade for the school / college
    public CodedString TALLPPEGRD_ALEV_1618_Est_Current { get; set; }

    // Academic qualifications Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_ACAD_1618_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Progress score for the school / college
    public CodedDouble VA_INS_ACAD_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Progress banding for the school / college
    public CodedString PROGRESS_BAND_ACAD_Est_Current { get; set; }

    // Academic qualifications Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_ACAD_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_ACAD_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Average result points for the school / college lower
    public CodedDouble TALLPPE_ACAD_1618_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Average result grade for the school / college
    public CodedString TALLPPEGRD_ACAD_1618_Est_Current { get; set; }

    // Applied general qualifications Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_AGEN_Est_Current_Num_Coded { get; set; }

    // Applied general qualifications Progress score for the school / college
    public CodedDouble VA_INS_AGEN_Est_Current_Num_Coded { get; set; }

    // Applied general qualifications Progress banding for the school / college
    public CodedString PROGRESS_BAND_AGEN_Est_Current { get; set; }

    // Applied general qualifications Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_AGEN_Est_Current_Num_Coded { get; set; }

    // Applied general qualifications Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_AGEN_Est_Current_Num_Coded { get; set; }

    // Applied general qualifications Average result points for the school / college lower
    public CodedDouble TALLPPE_AGEN_Est_Current_Num_Coded { get; set; }

    // Applied general qualifications Average result grade for the school / college
    public CodedString TALLPPEGRD_AGEN_Est_Current { get; set; }

    // Tech level Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_TLEV_Est_Current_Num_Coded { get; set; }

    // Tech level Progress score for the school / college
    public CodedDouble VA_INS_TLEV_Est_Current_Num_Coded { get; set; }

    // Tech level Progress banding for the school / college
    public CodedString PROGRESS_BAND_TLEV_Est_Current { get; set; }

    // Tech level Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_TLEV_Est_Current_Num_Coded { get; set; }

    // Tech level Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_TLEV_Est_Current_Num_Coded { get; set; }

    // Tech level Average result points for the school / college lower
    public CodedDouble TALLPPE_TLEV_Est_Current_Num_Coded { get; set; }

    // Tech level Average result grade for the school / college
    public CodedString TALLPPEGRD_TLEV_Est_Current { get; set; }

    // Tech Cert Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_TECHCERT_Est_Current_Num_Coded { get; set; }

    // Tech Cert Progress score for the school / college
    public CodedDouble VA_INS_TECHCERT_Est_Current_Num_Coded { get; set; }

    // Tech Cert Progress banding for the school / college
    public CodedString PROGRESS_BAND_TECHCERT_Est_Current { get; set; }

    // Tech Cert Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_TECHCERT_Est_Current_Num_Coded { get; set; }

    // Tech Cert Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_TECHCERT_Est_Current_Num_Coded { get; set; }

    // Tech Cert Average result points for the school / college lower
    public CodedDouble TALLPPE_TECHCERT_Est_Current_Num_Coded { get; set; }

    // Tech Cert Average result grade for the school / college
    public CodedString TALLPPEGRD_TECHCERT_Est_Current { get; set; }

    // A level additional data Number of students
    public CodedDouble TINCLUDE_B3_Est_Current_Num_Coded { get; set; }

    // A level additional data establishment points
    public CodedDouble TB3PTSE_Est_Current_Num_Coded { get; set; }

    // A level additional data establishment grade
    public CodedString TB3PTSE_GRD_Est_Current { get; set; }

    // Number of students for English progress
    public CodedDouble T_SCOPEEX_E_Est_Current_Num_Coded { get; set; }

    // Average progress in English for establishment
    public CodedDouble PROGEX_E_Est_Current_Num_Coded { get; set; }

    // Entered (pct) for English for establishment
    public CodedDouble ENTRY_PER_E_Est_Current_Pct_Coded { get; set; }

    // Number of students for Maths progress
    public CodedDouble T_SCOPEEX_M_Est_Current_Num_Coded { get; set; }

    // Average progress in Maths for establishment
    public CodedDouble PROGEX_M_Est_Current_Num_Coded { get; set; }

    // Entered (pct) for Maths for establishment
    public CodedDouble ENTRY_PER_M_Est_Current_Pct_Coded { get; set; }

    // Average progress for establishment - English - Disadvantaged
    public CodedDouble PROGEX_E_DIS_Est_Current_Num_Coded { get; set; }

    // Average progress for establishment - English - Disadvantaged
    public CodedDouble PROGEX_M_DIS_Est_Current_Num_Coded { get; set; }

    // Number of students for establishment - English - Disadvantaged
    public CodedDouble T_SCOPEEX_E_DIS_Est_Current_Num_Coded { get; set; }

    // Number of students for establishment - Maths - Disadvantaged
    public CodedDouble T_SCOPEEX_M_DIS_Est_Current_Num_Coded { get; set; }
}
