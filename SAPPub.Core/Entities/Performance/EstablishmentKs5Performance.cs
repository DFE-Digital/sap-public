using SAPPub.Core.ValueObjects;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.Performance;

public class EstablishmentKs5Performance
{
    public string Id { get; set; } = string.Empty;

    // Total number of students who completed at least one of this qualification type
    public CodedDouble TALLPUP_ACAD_1618_Est_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? TALLPUP_ACAD_1618_Est_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? TALLPUP_ACAD_1618_Est_Current_Num_Reason { get; set; }

    // Progress score for the school / college
    public CodedDouble VA_INS_ALEV_Est_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? VA_INS_ALEV_Est_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? VA_INS_ALEV_Est_Current_Num_Reason { get; set; }

    // Progress banding for the school / college
    public string? PROGRESS_BAND_ALEV_Est_Current { get; set; }

    // Progress confidence interval for the school / college upper
    public CodedDouble UCI_INS_ALEV_Est_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? UCI_INS_ALEV_Est_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? UCI_INS_ALEV_Est_Current_Num_Reason { get; set; }

    // Progress confidence interval for the school / college lower
    public CodedDouble LCI_INS_ALEV_Est_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? LCI_INS_ALEV_Est_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? LCI_INS_ALEV_Est_Current_Num_Reason { get; set; }

    // Average result points for the school / college lower
    public CodedDouble TALLPPE_ALEV_1618_Est_Current_Num_Coded { get; set; } = new();
    [IgnoreDataMember]
    public double? TALLPPE_ALEV_1618_Est_Current_Num { get; set; }
    [IgnoreDataMember]
    public string? TALLPPE_ALEV_1618_Est_Current_Num_Reason { get; set; }

    // Average result grade for the school / college
    public string? TALLPPEGRD_ALEV_1618_Est_Current { get; set; }    
}
