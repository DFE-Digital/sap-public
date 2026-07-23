using SAPPub.Core.ValueObjects;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.Performance;

public class LAKs5Performance
{
    public string Id { get; set; } = string.Empty;

    // Average result (points) for the LA state-funded schools / colleges
    public CodedDouble TALLPPE_ALEV_1618_LA_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? TALLPPE_ALEV_1618_LA_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? TALLPPE_ALEV_1618_LA_Current_Num_Reason { get; set; }

    // Average result (grade) for the LA state-funded schools / colleges
    public string? TALLPPEGRD_ALEV_1618_LA_Current_Num { get; set; }


    // Average progress in English for LA
    public CodedDouble PROGEX_E_LA_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? PROGEX_E_LA_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? PROGEX_E_LA_Current_Num_Reason { get; set; }


    // Entered for English qualifications
    public CodedDouble ENTRY_PER_E_LA_Current_Pct_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? ENTRY_PER_E_LA_Current_Pct { get; set; }
    [IgnoreDataMember]
    public string? ENTRY_PER_E_LA_Current_Pct_Reason { get; set; }


    // Average progress in Maths for LA
    public CodedDouble PROGEX_M_LA_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? PROGEX_M_LA_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? PROGEX_M_LA_Current_Num_Reason { get; set; }


    // Entered for Maths qualifications
    public CodedDouble ENTRY_PER_M_LA_Current_Pct_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? ENTRY_PER_M_LA_Current_Pct { get; set; }
    [IgnoreDataMember]
    public string? ENTRY_PER_M_LA_Current_Pct_Reason { get; set; }
}
