using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Destinations
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentDestinations
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// All Destinations Total filtered by Establishment for Current year
        /// </summary>
        public CodedDouble AllDest_Tot_Est_Current_Pct_Coded { get; set; }
        public double? AllDest_Tot_Est_Current_Pct { get; set; }
        public string AllDest_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Education Total filtered by Establishment for Current year
        /// </summary>
        public CodedDouble Education_Tot_Est_Current_Pct_Coded { get; set; }
        public double? Education_Tot_Est_Current_Pct { get; set; }
        public string Education_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Employment Total filtered by Establishment for Current year
        /// </summary>
        public CodedDouble Employment_Tot_Est_Current_Pct_Coded { get; set; }
        public double? Employment_Tot_Est_Current_Pct { get; set; }
        public string Employment_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// Apprenticeship Total filtered by Establishment for Current year
        /// </summary>
        public CodedDouble Apprentice_Tot_Est_Current_Pct_Coded { get; set; }
        public double? Apprentice_Tot_Est_Current_Pct { get; set; }
        public string Apprentice_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Destinations Total filtered by Establishment for Previous year
        /// </summary>
        public CodedDouble AllDest_Tot_Est_Previous_Pct_Coded { get; set; }
        public double? AllDest_Tot_Est_Previous_Pct { get; set; }
        public string AllDest_Tot_Est_Previous_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Education Total filtered by Establishment for Previous year
        /// </summary>
        public CodedDouble Education_Tot_Est_Previous_Pct_Coded { get; set; }
        public double? Education_Tot_Est_Previous_Pct { get; set; }
        public string Education_Tot_Est_Previous_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Employment Total filtered by Establishment for Previous year
        /// </summary>
        public CodedDouble Employment_Tot_Est_Previous_Pct_Coded { get; set; }
        public double? Employment_Tot_Est_Previous_Pct { get; set; }
        public string Employment_Tot_Est_Previous_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// Apprenticeship Total filtered by Establishment for Previous year
        /// </summary>
        public CodedDouble Apprentice_Tot_Est_Previous_Pct_Coded { get; set; }
        public double? Apprentice_Tot_Est_Previous_Pct { get; set; }
        public string Apprentice_Tot_Est_Previous_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Destinations Total filtered by Establishment for Previous2 year
        /// </summary>
        public CodedDouble AllDest_Tot_Est_Previous2_Pct_Coded { get; set; }
        public double? AllDest_Tot_Est_Previous2_Pct { get; set; }
        public string AllDest_Tot_Est_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Education Total filtered by Establishment for Previous2 year
        /// </summary>
        public CodedDouble Education_Tot_Est_Previous2_Pct_Coded { get; set; }
        public double? Education_Tot_Est_Previous2_Pct { get; set; }
        public string Education_Tot_Est_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// All Employment Total filtered by Establishment for Previous2 year
        /// </summary>
        public CodedDouble Employment_Tot_Est_Previous2_Pct_Coded { get; set; }
        public double? Employment_Tot_Est_Previous2_Pct { get; set; }
        public string Employment_Tot_Est_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// Apprenticeship Total filtered by Establishment for Previous2 year
        /// </summary>
        public CodedDouble Apprentice_Tot_Est_Previous2_Pct_Coded { get; set; }
        public double? Apprentice_Tot_Est_Previous2_Pct { get; set; }
        public string Apprentice_Tot_Est_Previous2_Pct_Reason { get; set; } = string.Empty;
    }
}
