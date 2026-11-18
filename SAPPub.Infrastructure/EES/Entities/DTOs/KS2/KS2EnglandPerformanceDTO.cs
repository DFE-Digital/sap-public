using EES.API.POC.KS2Performance.v1_x;
using SAPPub.Infrastructure.EES.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SAPPub.Infrastructure.EES.Entities.DTOs.KS2;

[ExcludeFromCodeCoverage]
public class KS2EnglandPerformanceDTO
{
    private const string CurrentTimePeriod = EESYearConstants.Year202425;
    private const string PreviousTimePeriod = EESYearConstants.Year202324;
    private const string Previous2TimePeriod = EESYearConstants.Year202223;
    public string Id { get; set; } = string.Empty;

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Maths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? MAT_AVERAGE_Eng_Current_Num_Coded { get; set; }
    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        Previous2TimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Maths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? MAT_AVERAGE_Eng_Previous2_Num_Coded { get; set; }
    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        PreviousTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Maths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? MAT_AVERAGE_Eng_Previous_Num_Coded { get; set; }
    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Maths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.Average2023To2025,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? MAT_AVERAGE_3YR_Eng_Current_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Maths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Disadvantaged,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Maths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.DisadvantagedAllOther,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.GrammarPunctuationAndSpelling,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTGPS_EXP_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.GrammarPunctuationAndSpelling,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTGPS_HIGH_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_EXP_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        Previous2TimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_EXP_Eng_Previous2_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        PreviousTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_EXP_Eng_Previous_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.Average2023To2025,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_EXP_3YR_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Disadvantaged,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Disadvantaged,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.DisadvantagedAllOther,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_HIGH_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        Previous2TimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_HIGH_Eng_Previous2_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        PreviousTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_HIGH_Eng_Previous_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.Average2023To2025,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_HIGH_3YR_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.ReadingWritingAndMaths,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.DisadvantagedAllOther,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Reading,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? READ_AVERAGE_Eng_Current_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        Previous2TimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Reading,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? READ_AVERAGE_Eng_Previous2_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        PreviousTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Reading,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.AllPupils,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? READ_AVERAGE_Eng_Previous_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Reading,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Total,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.Average2023To2025,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? READ_AVERAGE_3YR_Eng_Current_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Reading,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.Disadvantaged,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded { get; set; }

    [EESInput(
        KS2PerformanceLANAT.Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        KS2PerformanceLANAT.Filters.Subjects.Reading,
        KS2PerformanceLANAT.Filters.CharacteristicsOfEachGroup.DisadvantagedAllOther,
        KS2PerformanceLANAT.Filters.TopicOfCharacteristics.DisadvantagedStatus,
        KS2PerformanceLANAT.Filters.SchoolType.AllStateFunded
    )]
    public string? READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded { get; set; }
}
