using SAPPub.Infrastructure.EES.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static EES.API.POC.KS2Performance.v1_x.KS2PerformanceEstablishment;

namespace SAPPub.Infrastructure.EES.Entities.DTOs.KS2;

[ExcludeFromCodeCoverage]
public class KS2EstablishmentPerformanceDTO
{
    private const string CurrentTimePeriod = EESYearConstants.Year202425;
    private const string PreviousTimePeriod = EESYearConstants.Year202324;
    private const string Previous2TimePeriod = EESYearConstants.Year202223;

    public string Id { get; set; } = string.Empty;

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? MAT_AVERAGE_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        Previous2TimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? MAT_AVERAGE_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        PreviousTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? MAT_AVERAGE_Est_Previous_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup._3YearAverage
    )]
    public string? MAT_AVERAGE_3YR_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Boys
    )]
    public string? MAT_AVERAGE_B_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.KnownOrBelievedToBeOtherThanEnglish
    )]
    public string? MAT_AVERAGE_EAL_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Disadvantaged
    )]
    public string? MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Girls
    )]
    public string? MAT_AVERAGE_G_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.NonMobile
    )]
    public string? MAT_AVERAGE_MOBN_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasures,
        Previous2TimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Total,
        Filters.TopicOfCharacteristics.AllPupils
    )]

    public string? MATPROG_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasureLowerConfidenceLimitNotPopulatedIn2024,
        Previous2TimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? MATPROG_LOWER_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasureUpperConfidenceLimitNotPopulatedIn2024,
        Previous2TimePeriod,
        Filters.Subjects.Maths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? MATPROG_UPPER_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.GrammarPunctuationAndSpelling,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? PTGPS_EXP_Est_Current_Pct_Coded { get; set; }


    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.GrammarPunctuationAndSpelling,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? PTGPS_HIGH_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? PTRWM_EXP_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        Previous2TimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? PTRWM_EXP_Est_Previous2_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        PreviousTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? PTRWM_EXP_Est_Previous_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup._3YearAverage
        )]
    public string? PTRWM_EXP_3YR_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Boys
        )]
    public string? PTRWM_EXP_B_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.KnownOrBelievedToBeOtherThanEnglish
        )]
    public string? PTRWM_EXP_EAL_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Disadvantaged
    )]
    public string? PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Girls
    )]
    public string? PTRWM_EXP_G_Est_Current_Pct_Coded { get; set; }
    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheExpectedStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.NonMobile
    )]
    public string? PTRWM_EXP_MOBN_Est_Current_Pct_Coded { get; set; }
    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? PTRWM_HIGH_Est_Current_Pct_Coded { get; set; }
    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        Previous2TimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Total
    )]
    public string? PTRWM_HIGH_Est_Previous2_Pct_Coded { get; set; }
    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        PreviousTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? PTRWM_HIGH_Est_Previous_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup._3YearAverage
        )]
    public string? PTRWM_HIGH_3YR_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Boys
        )]
    public string? PTRWM_HIGH_B_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.KnownOrBelievedToBeOtherThanEnglish
        )]
    public string? PTRWM_HIGH_EAL_Est_Current_Pct_Coded { get; set; }
    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Disadvantaged
        )]
    public string? PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.Girls
        )]
    public string? PTRWM_HIGH_G_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfPupilsMeetingTheHigherStandard,
        CurrentTimePeriod,
        Filters.Subjects.ReadingWritingAndMaths,
        Filters.CharacteristicsOfEachGroup.NonMobile
        )]
    public string? PTRWM_HIGH_MOBN_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? READ_AVERAGE_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        Previous2TimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? READ_AVERAGE_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        PreviousTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? READ_AVERAGE_Est_Previous_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup._3YearAverage
        )]
    public string? READ_AVERAGE_3YR_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Boys
        )]
    public string? READ_AVERAGE_B_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.KnownOrBelievedToBeOtherThanEnglish
        )]
    public string? READ_AVERAGE_EAL_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Disadvantaged
        )]
    public string? READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Girls
        )]
    public string? READ_AVERAGE_G_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.AverageScaledScore,
        CurrentTimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.NonMobile
        )]
    public string? READ_AVERAGE_MOBN_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasures,
        Previous2TimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Total,
        Filters.TopicOfCharacteristics.AllPupils
        )]
    public string? READPROG_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasureLowerConfidenceLimitNotPopulatedIn2024,
        Previous2TimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? READPROG_LOWER_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasureUpperConfidenceLimitNotPopulatedIn2024,
        Previous2TimePeriod,
        Filters.Subjects.Reading,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? READPROG_UPPER_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasures,
        Previous2TimePeriod,
        Filters.Subjects.Writing,
        Filters.CharacteristicsOfEachGroup.Total,
        Filters.TopicOfCharacteristics.AllPupils
        )]
    public string? WRITPROG_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasureLowerConfidenceLimitNotPopulatedIn2024,
        Previous2TimePeriod,
        Filters.Subjects.Writing,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? WRITPROG_LOWER_Est_Previous2_Num_Coded { get; set; }

    [EESInput(
        Identifiers.ProgressMeasureUpperConfidenceLimitNotPopulatedIn2024,
        Previous2TimePeriod,
        Filters.Subjects.Writing,
        Filters.CharacteristicsOfEachGroup.Total
        )]
    public string? WRITPROG_UPPER_Est_Previous2_Num_Coded { get; set; }

}
