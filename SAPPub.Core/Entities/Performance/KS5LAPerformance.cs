using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Entities.Performance;

public class KS5LAPerformance
{
    public string Id { get; set; } = string.Empty;

    // A level Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_ALEV_1618_LA_Current_Num_Coded { get; set; }

    // A level Average result (grade) for the LA state-funded schools / colleges
    public CodedString TALLPPEGRD_ALEV_1618_LA_Current { get; set; }

    // Academic qualification Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_ACAD_1618_LA_Current_Num_Coded { get; set; }

    // Academic qualification Average result (grade) for the LA state-funded schools / colleges
    public CodedString TALLPPEGRD_ACAD_1618_LA_Current { get; set; }

    // Applied general qualifications Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_AGEN_LA_Current_Num_Coded { get; set; }

    // Applied general qualifications Average result (grade) for the LA state-funded schools / colleges
    public CodedString TALLPPEGRD_AGEN_LA_Current { get; set; }

    // T level Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_TLEV_LA_Current_Num_Coded { get; set; }

    // T level Average result (grade) for the LA state-funded schools / colleges
    public CodedString TALLPPEGRD_TLEV_LA_Current { get; set; }

    // Tech Cert Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_TECHCERT_LA_Current_Num_Coded { get; set; }

    // Tech CertAverage result (grade) for the LA state-funded schools / colleges
    public CodedString TALLPPEGRD_TECHCERT_LA_Current { get; set; }

    // A levels additional data LA points
    public CodedDouble TB3PTSE_LA_Current_Num_Coded { get; set; }

    // A levels additional data LA grade
    public CodedString TB3PTSE_GRD_LA_Current { get; set; }

    // Advanced level maths qualification percentage
    public CodedDouble L3M_PER_LA_Current_Pct_Coded { get; set; }

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

    // A Level DisAdvantaged

    // Number of students for LA - Disadvantaged - A Level
    public CodedDouble TALLPUP_ALEV_1618_DIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Disadvantaged - A Level
    public CodedDouble VA_INS_ALEV_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Disadvantaged - A Level
    public CodedDouble UCI_INS_ALEV_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Disadvantaged - A Level
    public CodedDouble LCI_INS_ALEV_DIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Disadvantaged - A Level
    public CodedString TALLPPEGRD_ALEV_DIS_LA_Current { get; set; }

    // Points for LA - Disadvantaged - A Level
    public CodedDouble TALLPPE_ALEV_1618_DIS_LA_Current_Num_Coded { get; set; }

    // A Level Non-DisAdvantaged

    // Number of students for LA - Non-Disadvantaged - A Level
    public CodedDouble TALLPUP_ALEV_1618_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Non-Disadvantaged - A Level
    public CodedDouble VA_INS_ALEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Non-Disadvantaged - A Level
    public CodedDouble UCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Non-Disadvantaged - A Level
    public CodedDouble LCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Non-Disadvantaged - A Level
    public CodedString TALLPPEGRD_ALEV_NOTDIS_LA_Current { get; set; }

    // Points for LA - Non-Disadvantaged - A Level
    public CodedDouble TALLPPE_ALEV_1618_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Academic qualifications DisAdvantaged

    // Number of students for LA - Disadvantaged - Academic qualifications
    public CodedDouble TALLPUP_ACAD_1618_DIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Disadvantaged - Academic qualifications
    public CodedDouble VA_INS_ACAD_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Disadvantaged - Academic qualifications
    public CodedDouble UCI_INS_ACAD_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Disadvantaged - Academic qualifications
    public CodedDouble LCI_INS_ACAD_DIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Disadvantaged - Academic qualifications
    public CodedString TALLPPEGRD_ACAD_DIS_LA_Current { get; set; }

    // Points for LA - Disadvantaged - Academic qualifications
    public CodedDouble TALLPPE_ACAD_1618_DIS_LA_Current_Num_Coded { get; set; }

    // Academic qualifications Non-DisAdvantaged

    // Number of students for LA - Non-Disadvantaged - Academic qualifications
    public CodedDouble TALLPUP_ACAD_1618_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Non-Disadvantaged - Academic qualifications
    public CodedDouble VA_INS_ACAD_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Non-Disadvantaged - Academic qualifications
    public CodedDouble UCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Non-Disadvantaged - Academic qualifications
    public CodedDouble LCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Non-Disadvantaged - Academic qualifications
    public CodedString TALLPPEGRD_ACAD_NOTDIS_LA_Current { get; set; }

    // Points for LA - Non-Disadvantaged - Academic qualifications
    public CodedDouble TALLPPE_ACAD_1618_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Applied general qualifications DisAdvantaged

    // Number of students for LA - Disadvantaged - Applied general qualifications
    public CodedDouble TALLPUP_AGEN_DIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Disadvantaged - Applied general qualifications
    public CodedDouble VA_INS_AGEN_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Disadvantaged - Applied general qualifications
    public CodedDouble UCI_INS_AGEN_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Disadvantaged - Applied general qualifications
    public CodedDouble LCI_INS_AGEN_DIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Disadvantaged - Applied general qualifications
    public CodedString TALLPPEGRD_AGEN_DIS_LA_Current { get; set; }

    // Points for LA - Disadvantaged - Applied general qualifications
    public CodedDouble TALLPPE_AGEN_DIS_LA_Current_Num_Coded { get; set; }

    // Applied general qualifications Non-DisAdvantaged

    // Number of students for LA - Non-Disadvantaged - Applied general qualifications
    public CodedDouble TALLPUP_AGEN_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Non-Disadvantaged - Applied general qualifications
    public CodedDouble VA_INS_AGEN_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Non-Disadvantaged - Applied general qualifications
    public CodedDouble UCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Non-Disadvantaged - Applied general qualifications
    public CodedDouble LCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Non-Disadvantaged - Applied general qualifications
    public CodedString TALLPPEGRD_AGEN_NOTDIS_LA_Current { get; set; }

    // Points for LA - Non-Disadvantaged - Applied general qualifications
    public CodedDouble TALLPPE_AGEN_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Tech Levels DisAdvantaged

    // Number of students for LA - Disadvantaged - Tech Levels
    public CodedDouble TALLPUP_TLEV_DIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Disadvantaged - Tech Levels
    public CodedDouble VA_INS_TLEV_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Disadvantaged - Tech Levels
    public CodedDouble UCI_INS_TLEV_DIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Disadvantaged - Tech Levels
    public CodedDouble LCI_INS_TLEV_DIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Disadvantaged - Tech Levels
    public CodedString TALLPPEGRD_TLEV_DIS_LA_Current { get; set; }

    // Points for LA - Disadvantaged - Tech Levels
    public CodedDouble TALLPPE_TLEV_DIS_LA_Current_Num_Coded { get; set; }

    // Tech Levels Non-DisAdvantaged

    // Number of students for LA - Non-Disadvantaged - Tech Levels
    public CodedDouble TALLPUP_TLEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress score for LA - Non-Disadvantaged - Tech Levels
    public CodedDouble VA_INS_TLEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA upper - Non-Disadvantaged - Tech Levels
    public CodedDouble UCI_INS_TLEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Progress confidence interval for LA lower - Non-Disadvantaged - Tech Levels
    public CodedDouble LCI_INS_TLEV_NOTDIS_LA_Current_Num_Coded { get; set; }

    // Grade for LA - Non-Disadvantaged - Tech Levels
    public CodedString TALLPPEGRD_TLEV_NOTDIS_LA_Current { get; set; }

    // Points for LA - Non-Disadvantaged - Tech Levels
    public CodedDouble TALLPPE_TLEV_NOTDIS_LA_Current_Num_Coded { get; set; }
}
