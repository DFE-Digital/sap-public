using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Destinations;

[ExcludeFromCodeCoverage]
public class KS4EnglandDestinations
{
    public string Id { get; set; } = string.Empty;

    //
    // All Destinations Total filtered by England for Current year
    //
    public CodedDouble AllDest_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // All Disadvantaged Destinations Total filtered by England for Current year
    //
    public CodedDouble AllDest_Dis_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // All Non-disadvantaged Destinations Total filtered by England for Current year
    //
    public CodedDouble AllDest_Ndis_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // All Education Total filtered by England for Current year
    //
    public CodedDouble Education_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // All Employment Total filtered by England for Current year
    //
    public CodedDouble Employment_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // Apprenticeship Total filtered by England for Current year
    //
    public CodedDouble Apprentice_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // Further Education Total filtered by England for Current year
    //
    public CodedDouble FurtherEd_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // School Sixth Form Total filtered by England for Current year
    //
    public CodedDouble SchSixthForm_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // Sixth Form College Total filtered by England for Current year
    //
    public CodedDouble ColSixthForm_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // Other Education Destinations Total filtered by England for Current year
    //
    public CodedDouble OtherEd_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // Not Sustained Total filtered by England for Current year
    //
    public CodedDouble NotSus_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // Unknown Destination Total filtered by England for Current year
    //
    public CodedDouble Unknown_Tot_Eng_Current_Pct_Coded { get; set; } = new();

    //
    // All Destinations Total filtered by England for Previous year
    //
    public CodedDouble AllDest_Tot_Eng_Previous_Pct_Coded { get; set; } = new();

    //
    // All Destinations Total filtered by England for Previous2 year
    //
    public CodedDouble AllDest_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();

}
