using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Absence;

[ExcludeFromCodeCoverage]
public class EnglandAbsence
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Persistent Absence Total filtered by England for Current year 
    /// </summary>
    public CodedDouble Abs_Persistent_Eng_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Absence Total filtered by England for Current year 
    /// </summary>
    public CodedDouble Abs_Tot_Eng_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_PersistentKS2_Eng_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_PersistentSPE_Eng_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_TotKS2_Eng_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_TotSPE_Eng_Current_Pct_Coded { get; set; }
}
