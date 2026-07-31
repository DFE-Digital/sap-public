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
    public string? TALLPPEGRD_ALEV_1618_Eng_Current { get; set; }

    // Academic qualifications Progress score for England average
    public CodedDouble VA_INS_ACAD_Eng_Current_Num_Coded { get; set; }

    // Academic qualifications Average result points for England
    public CodedDouble TALLPPE_ACAD_1618_Eng_Current_Num_Coded { get; set; }

    // Academic qualifications Average result grade for England
    public string? TALLPPEGRD_ACAD_1618_Eng_Current { get; set; }

    // Average progress for English
    public CodedDouble PROGEX_E_Eng_Current_Num_Coded { get; set; }

    // Entered for English qualifications ENG
    public CodedDouble ENTRY_PER_E_Eng_Current_Pct_Coded { get; set; }

    // Average progress for Maths
    public CodedDouble PROGEX_M_Eng_Current_Num_Coded { get; set; }

    // Entered for Maths qualifications ENG
    public CodedDouble ENTRY_PER_M_Eng_Current_Pct_Coded { get; set; }
}
