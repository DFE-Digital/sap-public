using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Absence
{
    [ExcludeFromCodeCoverage]
    public class LAAbsence
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Persistent Absence Total filtered by LA for Current year
        /// </summary>
        public CodedDouble Abs_Persistent_LA_Current_Pct_Coded { get; set; } = new();
        public double? Abs_Persistent_LA_Current_Pct { get; set; }
        public string Abs_Persistent_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// Absence Total filtered by LA for Current year
        /// </summary>
        public CodedDouble Abs_Tot_LA_Current_Pct_Coded { get; set; } = new();
        public double? Abs_Tot_LA_Current_Pct { get; set; }
        public string Abs_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// Auth Absence Total filtered by LA for Current year 
        /// </summary>
        public CodedDouble Auth_Tot_LA_Current_Pct_Coded { get; set; } = new();
        public double? Auth_Tot_LA_Current_Pct { get; set; }
        public string Auth_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// UnAuth Absence Total filtered by LA for Current year 
        /// </summary>
        public CodedDouble UnAuth_Tot_LA_Current_Pct_Coded { get; set; } = new();
        public double? UnAuth_Tot_LA_Current_Pct { get; set; }
        public string UnAuth_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;
    }
}
