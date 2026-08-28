using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Performance;

[ExcludeFromCodeCoverage]
public class KS5EnglandPerformance
{
    public string Id { get; set; } = string.Empty;

    // A level Progress score for England average
    public CodedDouble VA_INS_ALEV_Eng_Current_Num_Coded { get; set; }

    // Alevel Average result points for England
    public CodedDouble TALLPPE_ALEV_1618_Eng_Current_Num_Coded { get; set; }

    // A level Average result grade for England
    public CodedString TALLPPEGRD_ALEV_1618_Eng_Current { get; set; }

    // Academic qualifications Progress score for England average
    public CodedDouble VA_INS_ACAD_Eng_Current_Num_Coded { get; set; }

    // Academic qualifications Average result points for England
    public CodedDouble TALLPPE_ACAD_1618_Eng_Current_Num_Coded { get; set; }

    // Academic qualifications Average result grade for England
    public CodedString TALLPPEGRD_ACAD_1618_Eng_Current { get; set; }

    // Applied general qualifications Progress score for England average
    public CodedDouble VA_INS_AGEN_Eng_Current_Num_Coded { get; set; }

    // Applied general qualifications Average result points for England
    public CodedDouble TALLPPE_AGEN_Eng_Current_Num_Coded { get; set; }

    // Applied general qualifications Average result grade for England
    public CodedString TALLPPEGRD_AGEN_Eng_Current { get; set; }

    // Tech level Progress score for England average
    public CodedDouble VA_INS_TLEV_Eng_Current_Num_Coded { get; set; }

    // Tech level Average result points for England
    public CodedDouble TALLPPE_TLEV_Eng_Current_Num_Coded { get; set; }

    // Tech level Average result grade for England
    public CodedString TALLPPEGRD_TLEV_Eng_Current { get; set; }

    // Tech Cert Progress score for England average
    public CodedDouble VA_INS_TECHCERT_Eng_Current_Num_Coded { get; set; }

    // Tech Cert Average result points for England
    public CodedDouble TALLPPE_TECHCERT_Eng_Current_Num_Coded { get; set; }

    // Tech Cert Average result grade for England
    public CodedString TALLPPEGRD_TECHCERT_Eng_Current { get; set; }

    // A levels additional data england points
    public CodedDouble TB3PTSE_Eng_Current_Num_Coded { get; set; }

    // A levels additional data england grade
    public CodedString TB3PTSE_GRD_Eng_Current { get; set; }

    // Advanced level maths qualification percentage
    public CodedDouble L3M_PER_Eng_Current_Pct_Coded { get; set; }

    // Average progress for English
    public CodedDouble PROGEX_E_Eng_Current_Num_Coded { get; set; }

    // Entered for English qualifications ENG
    public CodedDouble ENTRY_PER_E_Eng_Current_Pct_Coded { get; set; }

    // Average progress for Maths
    public CodedDouble PROGEX_M_Eng_Current_Num_Coded { get; set; }

    // Entered for Maths qualifications ENG
    public CodedDouble ENTRY_PER_M_Eng_Current_Pct_Coded { get; set; }

    // Average progress for England
    // English - Disadvantaged
    public CodedDouble PROGEX_E_DIS_Eng_Current_Num_Coded { get; set; }
    // English - Not Dis.
    public CodedDouble PROGEX_E_NOTDIS_Eng_Current_Num_Coded { get; set; }
    // Maths - Disadvantaged
    public CodedDouble PROGEX_M_DIS_Eng_Current_Num_Coded { get; set; }
    // Maths - Not Dis.
    public CodedDouble PROGEX_M_NOTDIS_Eng_Current_Num_Coded { get; set; }


    //Number of students LA
    // English - Disadvantaged
    public CodedDouble T_SCOPEEX_E_DIS_Eng_Current_Num_Coded { get; set; }
    // English - Not Dis.
    public CodedDouble T_SCOPEEX_E_NOTDIS_Eng_Current_Num_Coded { get; set; }
    // Maths - Disadvantaged
    public CodedDouble T_SCOPEEX_M_DIS_Eng_Current_Num_Coded { get; set; }
    // Maths - Not Dis.
    public CodedDouble T_SCOPEEX_M_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // A Level DisAdvantaged

    // Number of students for England - Disadvantaged - A Level
    public CodedDouble TALLPUP_ALEV_1618_DIS_Eng_Current_Num_Coded { get; set; }

    // Progress score for England - Disadvantaged - A Level
    public CodedDouble VA_INS_ALEV_DIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England upper - Disadvantaged - A Level
    public CodedDouble UCI_INS_ALEV_DIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England lower - Disadvantaged - A Level
    public CodedDouble LCI_INS_ALEV_DIS_Eng_Current_Num_Coded { get; set; }

    // Grade for England - Disadvantaged - A Level
    public CodedString TALLPPEGRD_ALEV_DIS_Eng_Current { get; set; }

    // Points for England - Disadvantaged - A Level
    public CodedDouble TALLPPE_ALEV_1618_DIS_Eng_Current_Num_Coded { get; set; }

    // A Level Non-DisAdvantaged

    // Number of students for England - Non-Disadvantaged - A Level
    public CodedDouble TALLPUP_ALEV_1618_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Progress score for England - Non-Disadvantaged - A Level
    public CodedDouble VA_INS_ALEV_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England upper - Non-Disadvantaged - A Level
    public CodedDouble UCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England lower - Non-Disadvantaged - A Level
    public CodedDouble LCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Grade for England - Non-Disadvantaged - A Level
    public CodedString TALLPPEGRD_ALEV_NOTDIS_Eng_Current { get; set; }

    // Points for England - Non-Disadvantaged - A Level
    public CodedDouble TALLPPE_ALEV_1618_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Academic qualifications DisAdvantaged

    // Number of students for England - Disadvantaged - Academic qualifications
    public CodedDouble TALLPUP_ACAD_1618_DIS_Eng_Current_Num_Coded { get; set; }

    // Progress score for England - Disadvantaged - Academic qualifications
    public CodedDouble VA_INS_ACAD_DIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England upper - Disadvantaged - Academic qualifications
    public CodedDouble UCI_INS_ACAD_DIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England lower - Disadvantaged - Academic qualifications
    public CodedDouble LCI_INS_ACAD_DIS_Eng_Current_Num_Coded { get; set; }

    // Grade for England - Disadvantaged - Academic qualifications
    public CodedString TALLPPEGRD_ACAD_DIS_Eng_Current { get; set; }

    // Points for England - Disadvantaged - Academic qualifications
    public CodedDouble TALLPPE_ACAD_1618_DIS_Eng_Current_Num_Coded { get; set; }

    // Academic qualifications Non-DisAdvantaged

    // Number of students for England - Non-Disadvantaged - Academic qualifications
    public CodedDouble TALLPUP_ACAD_1618_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Progress score for England - Non-Disadvantaged - Academic qualifications
    public CodedDouble VA_INS_ACAD_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England upper - Non-Disadvantaged - Academic qualifications
    public CodedDouble UCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Progress confidence interval for England lower - Non-Disadvantaged - Academic qualifications
    public CodedDouble LCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded { get; set; }

    // Grade for England - Non-Disadvantaged - Academic qualifications
    public CodedString TALLPPEGRD_ACAD_NOTDIS_Eng_Current { get; set; }

    // Points for England - Non-Disadvantaged - Academic qualifications
    public CodedDouble TALLPPE_ACAD_1618_NOTDIS_Eng_Current_Num_Coded { get; set; }
}
