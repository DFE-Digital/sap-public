using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Destinations;

[ExcludeFromCodeCoverage]
public class KS4LADestinations
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// All Destinations Total filtered by LA for Current year
    /// </summary>
    public CodedDouble AllDest_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Disadvantaged Destinations Total filtered by LA for Current year
    /// </summary>
    public CodedDouble AllDest_Dis_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Non-disadvantaged Destinations Total filtered by LA for Current year
    /// </summary>
    public CodedDouble AllDest_Ndis_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Education Total filtered by LA for Current year
    /// </summary>
    public CodedDouble Education_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Employment Total filtered by LA for Current year
    /// </summary>
    public CodedDouble Employment_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Apprenticeship Total filtered by LA for Current year
    /// </summary>
    public CodedDouble Apprentice_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Further Education Total filtered by LA for Current year
    /// </summary>
    public CodedDouble FurtherEd_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// School Sixth Form Total filtered by LA for Current year
    /// </summary>
    public CodedDouble SchSixthForm_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Sixth Form College Total filtered by LA for Current year
    /// </summary>
    public CodedDouble ColSixthForm_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Other Education Destinations Total filtered by LA for Current year
    /// </summary>
    public CodedDouble OtherEd_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Not Sustained Total filtered by LA for Current year
    /// </summary>
    public CodedDouble NotSus_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// Unknown Destination Total filtered by LA for Current year
    /// </summary>
    public CodedDouble Unknown_Tot_LA_Current_Pct_Coded { get; set; }

    /// <summary>
    /// All Destinations Total filtered by LA for Previous year
    /// </summary>
    public CodedDouble AllDest_Tot_LA_Previous_Pct_Coded { get; set; }

    /// <summary>
    /// All Destinations Total filtered by LA for Previous2 year
    /// </summary>
    public CodedDouble AllDest_Tot_LA_Previous2_Pct_Coded { get; set; }

}
