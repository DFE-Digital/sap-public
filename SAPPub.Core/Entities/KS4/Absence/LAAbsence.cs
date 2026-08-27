using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Absence;

[ExcludeFromCodeCoverage]
public class LAAbsence
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Persistent Absence Total filtered by LA for Current year
    /// </summary>
    public CodedDouble Abs_Persistent_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Absence Total filtered by LA for Current year
    /// </summary>
    public CodedDouble Abs_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Auth Absence Total filtered by LA for Current year 
    /// </summary>
    public CodedDouble Auth_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// UnAuth Absence Total filtered by LA for Current year 
    /// </summary>
    public CodedDouble UnAuth_Tot_LA_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_TotKS2_LA_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_PersistentKS2_LA_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_TotSPE_LA_Current_Pct_Coded { get; set; }

    public CodedDouble Abs_PersistentSPE_LA_Current_Pct_Coded { get; set; }
}
