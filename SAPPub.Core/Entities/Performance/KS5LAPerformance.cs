using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Entities.Performance;

public class KS5LAPerformance
{
    public string Id { get; set; } = string.Empty;

    // A level Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_ALEV_1618_LA_Current_Num_Coded { get; set; }

    // A level Average result (grade) for the LA state-funded schools / colleges
    public string? TALLPPEGRD_ALEV_1618_LA_Current { get; set; }

    // Academic qualification Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_ACAD_1618_LA_Current_Num_Coded { get; set; }

    // Academic qualification Average result (grade) for the LA state-funded schools / colleges
    public string? TALLPPEGRD_ACAD_1618_LA_Current { get; set; }

    // Average progress in English for LA
    public CodedDouble PROGEX_E_LA_Current_Num_Coded { get; set; }

    // Entered for English qualifications
    public CodedDouble ENTRY_PER_E_LA_Current_Pct_Coded { get; set; }

    // Average progress in Maths for LA
    public CodedDouble PROGEX_M_LA_Current_Num_Coded { get; set; }

    // Entered for Maths qualifications
    public CodedDouble ENTRY_PER_M_LA_Current_Pct_Coded { get; set; }

    // Average progress for LA
    // English - Disadvantaged
    public CodedDouble PROGEX_E_DIS_LA_Current_Num_Coded { get; set; }
    // English - Not Dis.
    public CodedDouble PROGEX_E_NOTDIS_LA_Current_Num_Coded { get; set; }
    // Maths - Disadvantaged
    public CodedDouble PROGEX_M_DIS_LA_Current_Num_Coded { get; set; }
    // Maths - Not Dis.
    public CodedDouble PROGEX_M_NOTDIS_LA_Current_Num_Coded { get; set; } 

    //Number of students LA
    // English - Disadvantaged
    public CodedDouble T_SCOPEEX_E_DIS_LA_Current_Num_Coded { get; set; } 
    // English - Not Dis.
    public CodedDouble T_SCOPEEX_E_NOTDIS_LA_Current_Num_Coded { get; set; }
    // Maths - Disadvantaged
    public CodedDouble T_SCOPEEX_M_DIS_LA_Current_Num_Coded { get; set; }
    // Maths - Not Dis.
    public CodedDouble T_SCOPEEX_M_NOTDIS_LA_Current_Num_Coded { get; set; }

}
