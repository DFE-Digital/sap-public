using SAPPub.Core.ValueObjects; 
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Performance
{
    [ExcludeFromCodeCoverage]
    public class LAPerformance
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Attainment 8 Total filtered by LA for Current year
        /// <summary>
        public CodedDouble Attainment8_Tot_LA_Current_Num_Coded { get; set; } = new();
        public double? Attainment8_Tot_LA_Current_Num { get; set; }
        public string? Attainment8_Tot_LA_Current_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Boys filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths49_Boy_LA_Current_Pct_Coded { get; set; } = new();
        public double? EngMaths49_Boy_LA_Current_Pct { get; set; }
        public string? EngMaths49_Boy_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Girls filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths49_Grl_LA_Current_Pct_Coded { get; set; } = new();
        public double? EngMaths49_Grl_LA_Current_Pct { get; set; }
        public string? EngMaths49_Grl_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths49_Tot_LA_Current_Pct_Coded { get; set; } = new();
        public double? EngMaths49_Tot_LA_Current_Pct { get; set; }
        public string? EngMaths49_Tot_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Boys filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths59_Boy_LA_Current_Pct_Coded { get; set; } = new();
        public double? EngMaths59_Boy_LA_Current_Pct { get; set; }
        public string? EngMaths59_Boy_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Girls filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths59_Grl_LA_Current_Pct_Coded { get; set; } = new();
        public double? EngMaths59_Grl_LA_Current_Pct { get; set; }
        public string? EngMaths59_Grl_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths59_Tot_LA_Current_Pct_Coded { get; set; } = new();
        public double? EngMaths59_Tot_LA_Current_Pct { get; set; }
        public string? EngMaths59_Tot_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// Attainment 8 Total filtered by LA for Previous year
        /// <summary>
        public CodedDouble Attainment8_Tot_LA_Previous_Num_Coded { get; set; } = new();
        public double? Attainment8_Tot_LA_Previous_Num { get; set; }
        public string? Attainment8_Tot_LA_Previous_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by LA for Previous year
        /// <summary>
        public CodedDouble EngMaths49_Tot_LA_Previous_Pct_Coded { get; set; } = new();
        public double? EngMaths49_Tot_LA_Previous_Pct { get; set; }
        public string? EngMaths49_Tot_LA_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by LA for Previous year
        /// <summary>
        public CodedDouble EngMaths59_Tot_LA_Previous_Pct_Coded { get; set; } = new();
        public double? EngMaths59_Tot_LA_Previous_Pct { get; set; }
        public string? EngMaths59_Tot_LA_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// Progress 8 Average filtered by LA for Previous year
        /// <summary>
        public CodedDouble Prog8_Avg_LA_Previous_Num_Coded { get; set; } = new();
        public double? Prog8_Avg_LA_Previous_Num { get; set; }
        public string? Prog8_Avg_LA_Previous_Num_Reason { get; set; }

        /// <summary>
        /// Attainment 8 Total filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble Attainment8_Tot_LA_Previous2_Num_Coded { get; set; } = new();
        public double? Attainment8_Tot_LA_Previous2_Num { get; set; }
        public string? Attainment8_Tot_LA_Previous2_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble EngMaths49_Tot_LA_Previous2_Pct_Coded { get; set; } = new();
        public double? EngMaths49_Tot_LA_Previous2_Pct { get; set; }
        public string? EngMaths49_Tot_LA_Previous2_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble EngMaths59_Tot_LA_Previous2_Pct_Coded { get; set; } = new();
        public double? EngMaths59_Tot_LA_Previous2_Pct { get; set; }
        public string? EngMaths59_Tot_LA_Previous2_Pct_Reason { get; set; }

        /// <summary>
        /// Progress 8 Average filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble Prog8_Avg_LA_Previous2_Num_Coded { get; set; } = new();
        public double? Prog8_Avg_LA_Previous2_Num { get; set; }
        public string? Prog8_Avg_LA_Previous2_Num_Reason { get; set; }
    }
}
