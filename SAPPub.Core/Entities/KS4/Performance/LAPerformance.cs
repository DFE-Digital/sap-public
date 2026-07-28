using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

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
        [IgnoreDataMember]
        public double? Attainment8_Tot_LA_Current_Num { get; set; }
        [IgnoreDataMember]
        public string? Attainment8_Tot_LA_Current_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Boys filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths49_Boy_LA_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Boy_LA_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Boy_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Girls filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths49_Grl_LA_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Grl_LA_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Grl_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths49_Tot_LA_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Tot_LA_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Tot_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Boys filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths59_Boy_LA_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Boy_LA_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Boy_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Girls filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths59_Grl_LA_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Grl_LA_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Grl_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by LA for Current year
        /// <summary>
        public CodedDouble EngMaths59_Tot_LA_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Tot_LA_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Tot_LA_Current_Pct_Reason { get; set; }

        /// <summary>
        /// Attainment 8 Total filtered by LA for Previous year
        /// <summary>
        public CodedDouble Attainment8_Tot_LA_Previous_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Attainment8_Tot_LA_Previous_Num { get; set; }
        [IgnoreDataMember]
        public string? Attainment8_Tot_LA_Previous_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by LA for Previous year
        /// <summary>
        public CodedDouble EngMaths49_Tot_LA_Previous_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Tot_LA_Previous_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Tot_LA_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by LA for Previous year
        /// <summary>
        public CodedDouble EngMaths59_Tot_LA_Previous_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Tot_LA_Previous_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Tot_LA_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// Progress 8 Average filtered by LA for Previous year
        /// <summary>
        public CodedDouble Prog8_Avg_LA_Previous_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Prog8_Avg_LA_Previous_Num { get; set; }
        [IgnoreDataMember]
        public string? Prog8_Avg_LA_Previous_Num_Reason { get; set; }

        /// <summary>
        /// Attainment 8 Total filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble Attainment8_Tot_LA_Previous2_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Attainment8_Tot_LA_Previous2_Num { get; set; }
        [IgnoreDataMember]
        public string? Attainment8_Tot_LA_Previous2_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble EngMaths49_Tot_LA_Previous2_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Tot_LA_Previous2_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Tot_LA_Previous2_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble EngMaths59_Tot_LA_Previous2_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Tot_LA_Previous2_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Tot_LA_Previous2_Pct_Reason { get; set; }

        /// <summary>
        /// Progress 8 Average filtered by LA for Previous2 year
        /// <summary>
        public CodedDouble Prog8_Avg_LA_Previous2_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Prog8_Avg_LA_Previous2_Num { get; set; }
        [IgnoreDataMember]
        public string? Prog8_Avg_LA_Previous2_Num_Reason { get; set; }

        // Number of pupils at the end of KS4
        public CodedDouble Pup_Tot_LA_Current_Num_Coded { get; set; }

        ///
        /// Additional measures
        ///
        // Pupils achieving at least 1 qualification
        public CodedDouble AnyQual_Tot_LA_Current_Pct_Coded { get; set; }
        // Pupils entering for triple science
        public CodedDouble TripSci_Tot_LA_Current_Pct_Coded { get; set; }
        // Pupils entering for more than one foreign language
        public CodedDouble More1FL_Tot_LA_Current_Pct_Coded { get; set; }
        // Exam entries per pupil, GCSEs
        public CodedDouble ExamEntriesGSCE_Tot_LA_Current_Num_Coded { get; set; }
        // Exam entries per pupil, all KS4 qualifications
        public CodedDouble ExamEntriesKS4_Tot_LA_Current_Num_Coded { get; set; }
    }
}
