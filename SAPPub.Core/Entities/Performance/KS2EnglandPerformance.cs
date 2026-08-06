using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Performance;

[ExcludeFromCodeCoverage]
public class KS2EnglandPerformance
{
    public string Id { get; set; } = string.Empty;

    public CodedDouble MAT_AVERAGE_Eng_Current_Num_Coded { get; set; }
    public CodedDouble MAT_AVERAGE_Eng_Previous2_Num_Coded { get; set; }
    public CodedDouble MAT_AVERAGE_Eng_Previous_Num_Coded { get; set; }
    public CodedDouble MAT_AVERAGE_3YR_Eng_Current_Num_Coded { get; set; }
    public CodedDouble MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded { get; set; }
    public CodedDouble PTGPS_EXP_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTGPS_HIGH_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_EXP_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_EXP_Eng_Previous2_Pct_Coded { get; set; }
    public CodedDouble PTRWM_EXP_Eng_Previous_Pct_Coded { get; set; }
    public CodedDouble PTRWM_EXP_3YR_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_HIGH_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_HIGH_Eng_Previous2_Pct_Coded { get; set; }
    public CodedDouble PTRWM_HIGH_Eng_Previous_Pct_Coded { get; set; }
    public CodedDouble PTRWM_HIGH_3YR_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble READ_AVERAGE_Eng_Current_Num_Coded { get; set; }
    public CodedDouble READ_AVERAGE_Eng_Previous2_Num_Coded { get; set; }
    public CodedDouble READ_AVERAGE_Eng_Previous_Num_Coded { get; set; }
    public CodedDouble READ_AVERAGE_3YR_Eng_Current_Num_Coded { get; set; }
    public CodedDouble READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded { get; set; }
    public CodedDouble TFSM6CLA1A_Eng_Current_Num_Coded { get; set; }
    public CodedDouble TNOTFSM6CLA1A_Eng_Current_Num_Coded { get; set; }
    public CodedDouble PSENELE_Eng_Current_Pct_Coded { get; set; }
    public CodedDouble PSENELK_Eng_Current_Pct_Coded { get; set; }

}
