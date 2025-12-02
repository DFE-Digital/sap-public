using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.KS4.Absence
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentAbsence
    {
        public string Id { get; set; } = string.Empty;
        /// 
        /// Absence Total filtered by Establishment for Current year 
        /// 
        public double? Abs_Tot_Est_Current_Pct { get; set; }
        public string Abs_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// Absence Total filtered by Establishment for Current year 
        /// 
        public double Auth_Tot_Est_Current_Pct { get; set; }
        public string Auth_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// Absence Total filtered by Establishment for Current year 
        /// 
        public double UnAuth_Tot_Est_Current_Pct { get; set; }
        public string UnAuth_Tot_Est_Current_Pct_Reason { get; set; } = string.Empty;


    }
}
