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
}
