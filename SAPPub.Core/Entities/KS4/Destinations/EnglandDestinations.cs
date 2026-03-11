using SAPPub.Core.ValueObjects; // <-- whatever namespace your CodedDouble lives in
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Destinations
{
    [ExcludeFromCodeCoverage]
    public class EnglandDestinations
    {
        public string Id { get; set; } = string.Empty;

        //
        // All Destinations Total filtered by England for Current year
        //
        public CodedDouble AllDest_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        public double? AllDest_Tot_Eng_Current_Pct { get; set; }
        public string? AllDest_Tot_Eng_Current_Pct_Reason { get; set; }

        //
        // All Education Total filtered by England for Current year
        //
        public CodedDouble Education_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        public double? Education_Tot_Eng_Current_Pct { get; set; }
        public string? Education_Tot_Eng_Current_Pct_Reason { get; set; }

        //
        // All Employment Total filtered by England for Current year
        //
        public CodedDouble Employment_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        public double? Employment_Tot_Eng_Current_Pct { get; set; }
        public string? Employment_Tot_Eng_Current_Pct_Reason { get; set; }

        //
        // Apprenticeship Total filtered by England for Current year
        //
        public CodedDouble Apprentice_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        public double? Apprentice_Tot_Eng_Current_Pct { get; set; }
        public string? Apprentice_Tot_Eng_Current_Pct_Reason { get; set; }

        //
        // All Destinations Total filtered by England for Previous year
        //
        public CodedDouble AllDest_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        public double? AllDest_Tot_Eng_Previous_Pct { get; set; }
        public string? AllDest_Tot_Eng_Previous_Pct_Reason { get; set; }

        //
        // All Education Total filtered by England for Previous year
        //
        public CodedDouble Education_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        public double? Education_Tot_Eng_Previous_Pct { get; set; }
        public string? Education_Tot_Eng_Previous_Pct_Reason { get; set; }

        //
        // All Employment Total filtered by England for Previous year
        //
        public CodedDouble Employment_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        public double? Employment_Tot_Eng_Previous_Pct { get; set; }
        public string? Employment_Tot_Eng_Previous_Pct_Reason { get; set; }

        //
        // Apprenticeship Total filtered by England for Previous year
        //
        public CodedDouble Apprentice_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        public double? Apprentice_Tot_Eng_Previous_Pct { get; set; }
        public string? Apprentice_Tot_Eng_Previous_Pct_Reason { get; set; }

        //
        // All Destinations Total filtered by England for Previous2 year
        //
        public CodedDouble AllDest_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        public double? AllDest_Tot_Eng_Previous2_Pct { get; set; }
        public string? AllDest_Tot_Eng_Previous2_Pct_Reason { get; set; }

        //
        // All Education Total filtered by England for Previous2 year
        //
        public CodedDouble Education_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        public double? Education_Tot_Eng_Previous2_Pct { get; set; }
        public string? Education_Tot_Eng_Previous2_Pct_Reason { get; set; }

        //
        // All Employment Total filtered by England for Previous2 year
        //
        public CodedDouble Employment_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        public double? Employment_Tot_Eng_Previous2_Pct { get; set; }
        public string? Employment_Tot_Eng_Previous2_Pct_Reason { get; set; }

        //
        // Apprenticeship Total filtered by England for Previous2 year
        //
        public CodedDouble Apprentice_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        public double? Apprentice_Tot_Eng_Previous2_Pct { get; set; }
        public string? Apprentice_Tot_Eng_Previous2_Pct_Reason { get; set; }
    }
}
