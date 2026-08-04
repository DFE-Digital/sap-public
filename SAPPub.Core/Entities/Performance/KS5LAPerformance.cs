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

    // Applied general qualifications Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_AGEN_LA_Current_Num_Coded { get; set; }

    // Applied general qualifications Average result (grade) for the LA state-funded schools / colleges
    public string? TALLPPEGRD_AGEN_LA_Current { get; set; }

    // Average progress in English for LA
    public CodedDouble PROGEX_E_LA_Current_Num_Coded { get; set; }

    // Entered for English qualifications
    public CodedDouble ENTRY_PER_E_LA_Current_Pct_Coded { get; set; }

    // Average progress in Maths for LA
    public CodedDouble PROGEX_M_LA_Current_Num_Coded { get; set; }

    // Entered for Maths qualifications
    public CodedDouble ENTRY_PER_M_LA_Current_Pct_Coded { get; set; }
}
