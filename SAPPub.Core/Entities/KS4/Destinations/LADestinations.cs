using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.KS4.Destinations
{
    [ExcludeFromCodeCoverage]
    public class LADestinations
    {
        public string Id { get; set; } = string.Empty;
        /// 
        /// All Destinations Total filtered by LA for Current year 
        /// 
        public double? AllDest_Tot_LA_Current_Pct { get; set; }
        public string AllDest_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Education Total filtered by LA for Current year 
        /// 
        public double? Education_Tot_LA_Current_Pct { get; set; }
        public string Education_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Employment Total filtered by LA for Current year 
        /// 
        public double? Employment_Tot_LA_Current_Pct { get; set; }
        public string Employment_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// Apprenticeship Total filtered by LA for Current year 
        /// 
        public double? Apprentice_Tot_LA_Current_Pct { get; set; }
        public string Apprentice_Tot_LA_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Destinations Total filtered by LA for Previous year 
        /// 
        public double? AllDest_Tot_LA_Previous_Pct { get; set; }
        public string AllDest_Tot_LA_Previous_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Education Total filtered by LA for Previous year 
        /// 
        public double? Education_Tot_LA_Previous_Pct { get; set; }
        public string Education_Tot_LA_Previous_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Employment Total filtered by LA for Previous year 
        /// 
        public double? Employment_Tot_LA_Previous_Pct { get; set; }
        public string Employment_Tot_LA_Previous_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// Apprenticeship Total filtered by LA for Previous year 
        /// 
        public double? Apprentice_Tot_LA_Previous_Pct { get; set; }
        public string Apprentice_Tot_LA_Previous_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Destinations Total filtered by LA for Previous2 year 
        /// 
        public double? AllDest_Tot_LA_Previous2_Pct { get; set; }
        public string AllDest_Tot_LA_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Education Total filtered by LA for Previous2 year 
        /// 
        public double? Education_Tot_LA_Previous2_Pct { get; set; }
        public string Education_Tot_LA_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// All Employment Total filtered by LA for Previous2 year 
        /// 
        public double? Employment_Tot_LA_Previous2_Pct { get; set; }
        public string Employment_Tot_LA_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// Apprenticeship Total filtered by LA for Previous2 year 
        /// 
        public double? Apprentice_Tot_LA_Previous2_Pct { get; set; }
        public string Apprentice_Tot_LA_Previous2_Pct_Reason { get; set; } = string.Empty;




    }
}
