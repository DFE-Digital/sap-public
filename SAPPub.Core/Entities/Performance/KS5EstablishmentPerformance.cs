using SAPPub.Core.ValueObjects;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.Performance;

public class KS5EstablishmentPerformance
{
    public string Id { get; set; } = string.Empty;

    // Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_ALEV_1618_Est_Current_Num_Coded { get; set; }

    // A level Progress score for the school / college
    public CodedDouble VA_INS_ALEV_Est_Current_Num_Coded { get; set; }

    // A level Progress banding for the school / college
    public string? PROGRESS_BAND_ALEV_Est_Current { get; set; }

    // A level Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_ALEV_Est_Current_Num_Coded { get; set; }

    // A level Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_ALEV_Est_Current_Num_Coded { get; set; }

    // A level Average result points for the school / college lower
    public CodedDouble TALLPPE_ALEV_1618_Est_Current_Num_Coded { get; set; }

    // A level Average result grade for the school / college
    public string? TALLPPEGRD_ALEV_1618_Est_Current { get; set; }

    // Academic qualifications Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_ACAD_1618_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Progress score for the school / college
    public CodedDouble VA_INS_ACAD_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Progress banding for the school / college
    public string? PROGRESS_BAND_ACAD_Est_Current { get; set; }

    // Academic qualifications Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_ACAD_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_ACAD_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Average result points for the school / college lower
    public CodedDouble TALLPPE_ACAD_1618_Est_Current_Num_Coded { get; set; }

    // Academic qualifications Average result grade for the school / college
    public string? TALLPPEGRD_ACAD_1618_Est_Current { get; set; }

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
}
