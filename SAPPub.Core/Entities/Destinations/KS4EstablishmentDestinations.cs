using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Destinations;

[ExcludeFromCodeCoverage]
public class KS4EstablishmentDestinations
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// All Destinations Total filtered by Establishment for Current year
    /// </summary>
    public CodedDouble AllDest_Tot_Est_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Disadvantaged Destinations Total filtered by Establishment for Current year
    /// </summary>
    public CodedDouble AllDest_Dis_Est_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Education Total filtered by Establishment for Current year
    /// </summary>
    public CodedDouble Education_Tot_Est_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Employment Total filtered by Establishment for Current year
    /// </summary>
    public CodedDouble Employment_Tot_Est_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Apprenticeship Total filtered by Establishment for Current year
    /// </summary>
    public CodedDouble Apprentice_Tot_Est_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Destinations Total filtered by Establishment for Previous year
    /// </summary>
    public CodedDouble AllDest_Tot_Est_Previous_Pct_Coded { get; set; }

    /// <summary>
    /// All Destinations Total filtered by Establishment for Previous2 year
    /// </summary>
    public CodedDouble AllDest_Tot_Est_Previous2_Pct_Coded { get; set; }

}
