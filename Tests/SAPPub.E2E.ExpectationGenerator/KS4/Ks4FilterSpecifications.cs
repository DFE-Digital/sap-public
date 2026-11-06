using static SAPPub.E2E.TestDataExpectationGenerator.KS4.FilterSpecification;

namespace SAPPub.E2E.TestDataExpectationGenerator.KS4;

/// <summary>
/// The filters behind each existing KS4 expectation JSON file in TestData/2526/KS4.
/// Each Create method returns one <see cref="FilterSpecification"/> per file for a given source CSV.
/// </summary>
internal static class Ks4FilterSpecifications
{
    private static readonly (string, string) CurrentPeriod = ("time_period", "202526");
    private static readonly (string, string) PreviousPeriod = ("time_period", "202425");
    private static readonly (string, string) Previous2Period = ("time_period", "202324");

    private static readonly (string, string) Total = ("breakdown_topic", "Total");
    private static readonly (string, string) SexTopic = ("breakdown_topic", "Sex");
    private static readonly (string, string) DisadvantageTopic = ("breakdown_topic", "Disadvantage status");
    private static readonly (string, string) FirstLanguageTopic = ("breakdown_topic", "First language");
    private static readonly (string, string) MobilityTopic = ("breakdown_topic", "Mobility");

    private static readonly (string, string) Boys = ("sex", "Boys");
    private static readonly (string, string) Girls = ("sex", "Girls");
    private static readonly (string, string) Disadvantaged = ("disadvantage_status", "Disadvantaged");
    private static readonly (string, string) NonDisadvantaged = ("disadvantage_status", "Not known to be disadvantaged");
    private static readonly (string, string) Eal = ("first_language", "Known or believed to be other than English");
    private static readonly (string, string) NonMobile = ("mobility", "Non mobile");

    public static IReadOnlyList<FilterSpecification> CreatePerformanceTables() =>
    [
        Create("performance_tables_schools_final_Total_Current_Year", CurrentPeriod, Total),
        Create("performance_tables_schools_final_Total_Previous_Year", PreviousPeriod, Total),
        Create("performance_tables_schools_final_Total_Previous2_Year", Previous2Period, Total),
        Create("performance_tables_schools_final_Boys_Current_Year", CurrentPeriod, SexTopic, Boys),
        Create("performance_tables_schools_final_Girls_Current_Year", CurrentPeriod, SexTopic, Girls),
        Create("performance_tables_schools_final_Disadvantaged_Current_Year", CurrentPeriod, DisadvantageTopic, Disadvantaged),
        Create("performance_tables_schools_final_Disadvantaged_Previous_Year", PreviousPeriod, DisadvantageTopic, Disadvantaged),
        Create("performance_tables_schools_final_Disadvantaged_Previous2_Year", Previous2Period, DisadvantageTopic, Disadvantaged),
        Create("performance_tables_schools_final_EAL_Current_Year", CurrentPeriod, FirstLanguageTopic, Eal),
        Create("performance_tables_schools_final_NonMobile_Current_Year", CurrentPeriod, MobilityTopic, NonMobile)
    ];

    public static IReadOnlyList<FilterSpecification> CreateAllStateLa() =>
    [
        Create("all_state_funded_pupils_characteristics_la_Total_Current", CurrentPeriod, Total),
        Create("all_state_funded_pupils_characteristics_la_Total_Previous", PreviousPeriod, Total),
        Create("all_state_funded_pupils_characteristics_la_Total_Previous2", Previous2Period, Total),
        Create("all_state_funded_pupils_characteristics_la_Boys_Current", CurrentPeriod, SexTopic, Boys),
        Create("all_state_funded_pupils_characteristics_la_Girls_Current", CurrentPeriod, SexTopic, Girls),
        Create("all_state_funded_pupils_characteristics_la_Disadvantaged_Current", CurrentPeriod, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_la_Disadvantaged_Previous", PreviousPeriod, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_la_Disadvantaged_Previous2", Previous2Period, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_la_NonDisadvantaged_Current", CurrentPeriod, DisadvantageTopic, NonDisadvantaged)
    ];

    public static IReadOnlyList<FilterSpecification> CreateAllStateEngland() =>
    [
        Create("all_state_funded_pupils_characteristics_England_Total_Current", CurrentPeriod, Total),
        Create("all_state_funded_pupils_characteristics_England_Total_Previous", PreviousPeriod, Total),
        Create("all_state_funded_pupils_characteristics_England_Total_Previous2", Previous2Period, Total),
        Create("all_state_funded_pupils_characteristics_England_Disadvantaged_Current", CurrentPeriod, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_England_NonDisadvantaged_Current", CurrentPeriod, DisadvantageTopic, NonDisadvantaged)
    ];

    public static IReadOnlyList<FilterSpecification> CreateInformationAboutSchools() =>
    [
        Create("information_about_schools_Current", CurrentPeriod),
        Create("information_about_schools_Previous", PreviousPeriod),
        Create("information_about_schools_Previous2", Previous2Period)
    ];
}
