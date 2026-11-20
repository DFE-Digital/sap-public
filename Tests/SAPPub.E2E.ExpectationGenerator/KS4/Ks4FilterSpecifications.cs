using Microsoft.AspNetCore.Http;
using static SAPPub.E2E.TestDataExpectationGenerator.KS4.FilterSpecification;

namespace SAPPub.E2E.TestDataExpectationGenerator.KS4;

/// <summary>
/// The filters behind each existing KS4 expectation JSON file in TestData/2526/KS4.
/// Each Create method returns one <see cref="FilterSpecification"/> per file for a given source CSV.
/// </summary>
internal static class Ks4FilterSpecifications
{
    // using the same urns for every test file
    private static readonly (string, string[]) SchoolUrns = 
            ("school_urn", ["124856", "136489", "136791", "137205", "137252", "138765", "140424"]); 

    private static readonly (string, string) ExamTotals = ("grade", "Total exam entries");
    private static readonly (string, string) QualificationGcse = ("qualification_type", "GCSE");
    private static readonly (string, string) QualificationVocational = ("qualification_type", "Vocational");
    private static readonly (string, string) QualificationOther = ("qualification_type", "FSMQ");

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

    private static readonly (string, string) DestinationsCurrent = ("time_period", "202324");
    private static readonly (string, string) DestinationsPrevious = ("time_period", "202223");
    private static readonly (string, string) DestinationsPrevious2 = ("time_period", "202122");
    private static readonly (string, string[]) DestinationsPast = ("time_period", ["202223", "202122"]);
    private static readonly (string, string) DestTotalTopic = ("breakdown_topic", "Total");
    private static readonly (string, string) DestTotalBreakdown = ("breakdown", "Total");
    private static readonly (string, string) DestTotalSex = ("sex", "Total");
    private static readonly (string, string) DestTotalEthnicity = ("ethnicity_major", "Total");
    private static readonly (string, string) DestDisadvantageTotal = ("disadvantage_status", "Total");
    private static readonly (string, string[]) DestDisadvantageSplit = ("disadvantage_status", ["Disadvantaged", "Not known to be disadvantaged"]);
    private static readonly (string, string) DestOverall = ("destination_group", "Overall destination");
    private static readonly (string, string[]) DestAllGroups = ("destination_group", ["Overall destination", "Headline destinations", "Education destination breakdown"]);

    private static IReadOnlyList<FilterSpecification> CreateDestinations(string outputFileName, params FilterCriterion[] level) =>
    [
        Create(outputFileName, [.. level, DestTotalTopic, DestTotalBreakdown, DestTotalSex, DestTotalEthnicity, DestinationsCurrent, DestDisadvantageTotal, DestAllGroups]),
        Create(outputFileName, [.. level, DestTotalTopic, DestTotalBreakdown, DestTotalSex, DestTotalEthnicity, DestinationsCurrent, DestDisadvantageSplit, DestOverall]),
        Create(outputFileName, [.. level, DestTotalTopic, DestTotalBreakdown, DestTotalSex, DestTotalEthnicity, DestinationsPrevious, DestDisadvantageTotal, DestOverall]),
        Create(outputFileName, [.. level, DestTotalTopic, DestTotalBreakdown, DestTotalSex, DestTotalEthnicity, DestinationsPrevious2, DestDisadvantageTotal, DestOverall]),
    ];

    public static IReadOnlyList<FilterSpecification> CreateDestinationsEstablishment() =>
        CreateDestinations("ees_ks4_202324_api_establishment", SchoolUrns, ("geographic_level", "School"));

    public static IReadOnlyList<FilterSpecification> CreateDestinationsLa() =>
        CreateDestinations(
            "ees_ks4_202324_api_la",
            SchoolUrns,
            ("geographic_level", "Local authority"),
            ("establishment_type_group", "State-funded mainstream and special"));

    public static IReadOnlyList<FilterSpecification> CreateDestinationsEngland() =>
        CreateDestinations(
            "ees_ks4_202324_api_england",
            ("geographic_level", "National"),
            ("establishment_type_group", "Total state-funded mainstream and special"));

    public static IReadOnlyList<FilterSpecification> CreateSubjectGcseEntries() =>
    [
        Create("SubjectsEntered", SchoolUrns, CurrentPeriod, ExamTotals, QualificationGcse),
    ];

    public static IReadOnlyList<FilterSpecification> CreateSubjectVocationalEntries() =>
    [
        Create("SubjectsEntered", SchoolUrns, CurrentPeriod, ExamTotals, QualificationVocational)
    ];

    public static IReadOnlyList<FilterSpecification> CreateSubjectOtherEntries() =>
    [
        Create("SubjectsEntered", SchoolUrns, CurrentPeriod, ExamTotals, QualificationOther)
    ];

    public static IReadOnlyList<FilterSpecification> CreatePerformanceTables() =>
    [
        Create("performance_tables_schools_final_Total_Current_Year", SchoolUrns, CurrentPeriod, Total),
        Create("performance_tables_schools_final_Total_Previous_Year", SchoolUrns, PreviousPeriod, Total),
        Create("performance_tables_schools_final_Total_Previous2_Year", SchoolUrns, Previous2Period, Total),
        Create("performance_tables_schools_final_Boys_Current_Year", SchoolUrns, CurrentPeriod, SexTopic, Boys),
        Create("performance_tables_schools_final_Girls_Current_Year", SchoolUrns, CurrentPeriod, SexTopic, Girls),
        Create("performance_tables_schools_final_Disadvantaged_Current_Year", SchoolUrns, CurrentPeriod, DisadvantageTopic, Disadvantaged),
        Create("performance_tables_schools_final_Disadvantaged_Previous_Year", SchoolUrns, PreviousPeriod, DisadvantageTopic, Disadvantaged),
        Create("performance_tables_schools_final_Disadvantaged_Previous2_Year", SchoolUrns, Previous2Period, DisadvantageTopic, Disadvantaged),
        Create("performance_tables_schools_final_EAL_Current_Year", SchoolUrns, CurrentPeriod, FirstLanguageTopic, Eal),
        Create("performance_tables_schools_final_NonMobile_Current_Year", SchoolUrns, CurrentPeriod, MobilityTopic, NonMobile)
    ];

    public static IReadOnlyList<FilterSpecification> CreateAllStateLa() =>
    [
        Create("all_state_funded_pupils_characteristics_la_Total_Current", SchoolUrns, CurrentPeriod, Total),
        Create("all_state_funded_pupils_characteristics_la_Total_Previous", SchoolUrns, PreviousPeriod, Total),
        Create("all_state_funded_pupils_characteristics_la_Total_Previous2", SchoolUrns, Previous2Period, Total),
        Create("all_state_funded_pupils_characteristics_la_Boys_Current", SchoolUrns, CurrentPeriod, SexTopic, Boys),
        Create("all_state_funded_pupils_characteristics_la_Girls_Current", SchoolUrns, CurrentPeriod, SexTopic, Girls),
        Create("all_state_funded_pupils_characteristics_la_Disadvantaged_Current", SchoolUrns, CurrentPeriod, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_la_Disadvantaged_Previous", SchoolUrns, PreviousPeriod, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_la_Disadvantaged_Previous2", SchoolUrns, Previous2Period, DisadvantageTopic, Disadvantaged),
        Create("all_state_funded_pupils_characteristics_la_NonDisadvantaged_Current", SchoolUrns, CurrentPeriod, DisadvantageTopic, NonDisadvantaged)
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
        Create("information_about_schools_Current", SchoolUrns, CurrentPeriod),
        Create("information_about_schools_Previous", SchoolUrns, PreviousPeriod),
        Create("information_about_schools_Previous2", SchoolUrns, Previous2Period)
    ];
}
