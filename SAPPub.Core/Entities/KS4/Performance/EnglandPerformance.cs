using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.KS4.Performance
{
    [ExcludeFromCodeCoverage]
    public class EnglandPerformance
    {
        /// 
        /// Attainment 8 Total filtered by England for Current year 
        /// 
        public double Attainment8_Tot_Eng_Current_Num { get; set; }
        public string Attainment8_Tot_Eng_Current_Num_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 4 to 9 Boys filtered by England for Current year 
        /// 
        public double EngMaths49_Boy_Eng_Current_Pct { get; set; }
        public string EngMaths49_Boy_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 4 to 9 Girls filtered by England for Current year 
        /// 
        public double EngMaths49_Grl_Eng_Current_Pct { get; set; }
        public string EngMaths49_Grl_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 4 to 9 Total filtered by England for Current year 
        /// 
        public double EngMaths49_Tot_Eng_Current_Pct { get; set; }
        public string EngMaths49_Tot_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 5 to 9 Boys filtered by England for Current year 
        /// 
        public double EngMaths59_Boy_Eng_Current_Pct { get; set; }
        public string EngMaths59_Boy_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 5 to 9 Girls filtered by England for Current year 
        /// 
        public double EngMaths59_Grl_Eng_Current_Pct { get; set; }
        public string EngMaths59_Grl_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 5 to 9 Total filtered by England for Current year 
        /// 
        public double EngMaths59_Tot_Eng_Current_Pct { get; set; }
        public string EngMaths59_Tot_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 4 to 9 Total filtered by England for Previous year 
        /// 
        public double EngMaths49_Tot_Eng_Previous_Pct { get; set; }
        public string EngMaths49_Tot_Eng_Previous_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 5 to 9 Total filtered by England for Previous year 
        /// 
        public double EngMaths59_Tot_Eng_Previous_Pct { get; set; }
        public string EngMaths59_Tot_Eng_Previous_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 4 to 9 Total filtered by England for Previous2 year 
        /// 
        public double EngMaths49_Tot_Eng_Previous2_Pct { get; set; }
        public string EngMaths49_Tot_Eng_Previous2_Pct_Reason { get; set; } = string.Empty;

        /// 
        /// English and Maths grades 5 to 9 Total filtered by England for Previous2 year 
        /// 
        public double EngMaths59_Tot_Eng_Previous2_Pct { get; set; }
        public string EngMaths59_Tot_Eng_Previous2_Pct_Reason { get; set; } = string.Empty;


    }
}
