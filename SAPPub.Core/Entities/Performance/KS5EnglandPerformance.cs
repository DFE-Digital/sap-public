using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.Performance;

[ExcludeFromCodeCoverage]
public class KS5EnglandPerformance
{
    public string Id { get; set; } = string.Empty;

    // Progress score for England average
    public CodedDouble VA_INS_ALEV_Eng_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? VA_INS_ALEV_Eng_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? VA_INS_ALEV_Eng_Current_Num_Reason { get; set; }

    // Average result points for England
    public CodedDouble TALLPPE_ALEV_1618_Eng_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? TALLPPE_ALEV_1618_Eng_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? TALLPPE_ALEV_1618_Eng_Current_Num_Reason { get; set; }

    // Average result grade for England
    public string? TALLPPEGRD_ALEV_1618_Eng_Current { get; set; }    

    // Average progress for English
    public CodedDouble PROGEX_E_Eng_Current_Num_Coded { get; set; } = new();

    // Entered for English qualifications ENG
    public CodedDouble ENTRY_PER_E_Eng_Current_Pct_Coded { get; set; } = new();

    // Average progress for Maths
    public CodedDouble PROGEX_M_Eng_Current_Num_Coded { get; set; } = new();

    // Entered for Maths qualifications ENG
    public CodedDouble ENTRY_PER_M_Eng_Current_Pct_Coded { get; set; } = new();

}
