using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Absence;

[ExcludeFromCodeCoverage]
public class EstablishmentAbsence
{
    // Keys / context
    public string Id { get; set; } = string.Empty;

    public CodedDouble Enrolments_Tot_Est_Current_Num_Coded { get; set; }   // Total Enrolments
    public CodedDouble Abs_Persistent_Est_Current_Num_Coded { get; set; }   // Persistent absence %
    public CodedDouble Abs_Persistent_Est_Current_Pct_Coded { get; set; }
    public CodedDouble Abs_Tot_Est_Current_Pct_Coded { get; set; }          // Overall absence %
    public CodedDouble Abs_PersistentKS2_Est_Current_Num_Coded { get; set; }
    public CodedDouble Abs_PersistentKS2_Est_Current_Pct_Coded { get; set; }
    public CodedDouble Abs_PersistentSPE_Est_Current_Num_Coded { get; set; }
    public CodedDouble Abs_PersistentSPE_Est_Current_Pct_Coded { get; set; }
    public CodedDouble Abs_TotKS2_Est_Current_Pct_Coded { get; set; }
    public CodedDouble Abs_TotSPE_Est_Current_Pct_Coded { get; set; }
    public CodedDouble Enrolments_TotKS2_Est_Current_Num_Coded { get; set; }
    public CodedDouble Enrolments_TotSPE_Est_Current_Num_Coded { get; set; }
}