using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.KS4.Performance
{
    [ExcludeFromCodeCoverage]
    public class EnglandPerformance
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Attainment 8 Total filtered by England for Current year
        /// <summary>
        public CodedDouble Attainment8_Tot_Eng_Current_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Attainment8_Tot_Eng_Current_Num { get; set; }
        [IgnoreDataMember]
        public string? Attainment8_Tot_Eng_Current_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Boys filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths49_Boy_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Boy_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Boy_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Girls filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths49_Grl_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Grl_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Grl_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Disadvantaged filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths49_Dis_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Dis_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Dis_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 non-disadvantaged filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths49_NDi_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_NDi_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_NDi_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths49_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Tot_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Tot_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Boys filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths59_Boy_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Boy_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Boy_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Girls filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths59_Grl_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Grl_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Grl_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 disadvantaged filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths59_Dis_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Dis_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Dis_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 non-disadvantaged filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths59_NDi_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_NDi_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_NDi_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths59_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Tot_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Tot_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 Boys filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths79_Boy_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_Boy_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_Boy_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 Girls filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths79_Grl_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_Grl_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_Grl_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 disadvantaged filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths79_Dis_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_Dis_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_Dis_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 non-disadvantaged filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths79_NDi_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_NDi_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_NDi_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 Total filtered by England for Current year
        /// <summary>
        public CodedDouble EngMaths79_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_Tot_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_Tot_Eng_Current_Pct_Reason { get; set; }

        /// <summary>
        /// Attainment 8 Total filtered by England for Previous year
        /// <summary>
        public CodedDouble Attainment8_Tot_Eng_Previous_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Attainment8_Tot_Eng_Previous_Num { get; set; }
        [IgnoreDataMember]
        public string? Attainment8_Tot_Eng_Previous_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by England for Previous year
        /// <summary>
        public CodedDouble EngMaths49_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Tot_Eng_Previous_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Tot_Eng_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by England for Previous year
        /// <summary>
        public CodedDouble EngMaths59_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Tot_Eng_Previous_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Tot_Eng_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 Total filtered by England for Previous year
        /// <summary>
        public CodedDouble EngMaths79_Tot_Eng_Previous_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_Tot_Eng_Previous_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_Tot_Eng_Previous_Pct_Reason { get; set; }

        /// <summary>
        /// Attainment 8 Total filtered by England for Previous2 year
        /// <summary>
        public CodedDouble Attainment8_Tot_Eng_Previous2_Num_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Attainment8_Tot_Eng_Previous2_Num { get; set; }
        [IgnoreDataMember]
        public string? Attainment8_Tot_Eng_Previous2_Num_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 4 to 9 Total filtered by England for Previous2 year
        /// <summary>
        public CodedDouble EngMaths49_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths49_Tot_Eng_Previous2_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths49_Tot_Eng_Previous2_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 5 to 9 Total filtered by England for Previous2 year
        /// <summary>
        public CodedDouble EngMaths59_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths59_Tot_Eng_Previous2_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths59_Tot_Eng_Previous2_Pct_Reason { get; set; }

        /// <summary>
        /// English and Maths grades 7 to 9 Total filtered by England for Previous2 year
        /// <summary>
        public CodedDouble EngMaths79_Tot_Eng_Previous2_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? EngMaths79_Tot_Eng_Previous2_Pct { get; set; }
        [IgnoreDataMember]
        public string? EngMaths79_Tot_Eng_Previous2_Pct_Reason { get; set; }


        // Number of pupils at the end of KS4
        public CodedDouble Pup_Tot_Eng_Current_Num_Coded { get; set; }

        ///
        /// Additional measures
        ///
        // Pupils achieving at least 1 qualification
        public CodedDouble AnyQual_Tot_Eng_Current_Pct_Coded { get; set; }
        // Pupils entering for triple science
        public CodedDouble TripSci_Tot_Eng_Current_Pct_Coded { get; set; }
        // Pupils entering for more than one foreign language
        public CodedDouble More1FL_Tot_Eng_Current_Pct_Coded { get; set; }
        // Exam entries per pupil, GCSEs
        public CodedDouble ExamEntriesGSCE_Tot_Eng_Current_Num_Coded { get; set; }
        // Exam entries per pupil, all KS4 qualifications
        public CodedDouble ExamEntriesKS4_Tot_Eng_Current_Num_Coded { get; set; }

        public static RelativeYearValues<double?> AllEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
        {
            return new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Tot_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Tot_Eng_Current_Pct,
                    7 => englandPerformance.EngMaths79_Tot_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Tot_Eng_Previous_Pct,
                    5 => englandPerformance.EngMaths59_Tot_Eng_Previous_Pct,
                    7 => englandPerformance.EngMaths79_Tot_Eng_Previous_Pct,
                    _ => null
                },
                TwoYearsAgo = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Tot_Eng_Previous2_Pct,
                    5 => englandPerformance.EngMaths59_Tot_Eng_Previous2_Pct,
                    7 => englandPerformance.EngMaths79_Tot_Eng_Previous2_Pct,
                    _ => null
                }
            };
        }

        public static RelativeYearValues<double?> BoysEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
        {
            return new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Boy_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Boy_Eng_Current_Pct,
                    7 => englandPerformance.EngMaths79_Boy_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            };
        }

        public static RelativeYearValues<double?> GirlsEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
        {
            return new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Grl_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Grl_Eng_Current_Pct,
                    7 => englandPerformance.EngMaths79_Grl_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            };
        }

        public static RelativeYearValues<double?> DisadvantagedEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
        {
            return new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Dis_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Dis_Eng_Current_Pct,
                    7 => englandPerformance.EngMaths79_Dis_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            };
        }

        public static RelativeYearValues<double?> NonDisadvantagedEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
        {
            return new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_NDi_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_NDi_Eng_Current_Pct,
                    7 => englandPerformance.EngMaths79_NDi_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            };
        }
    }
}