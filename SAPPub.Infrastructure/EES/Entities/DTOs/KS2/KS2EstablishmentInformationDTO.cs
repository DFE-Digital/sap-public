using SAPPub.Infrastructure.EES.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static EES.API.POC.KS2Information.v1_x.KS2InformationEstablishment;

namespace SAPPub.Infrastructure.EES.Entities.DTOs.KS2;

[ExcludeFromCodeCoverage]
public class KS2EstablishmentInformationDTO
{
    private const string CurrentTimePeriod = EESYearConstants.Year202425;

    public string Id { get; set; } = string.Empty;


    [EESInput(
        Identifiers.EligibleBoysOnSchoolRollAtTimeOfTests,
        CurrentTimePeriod
    )]
    public string? BELIG_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.EligibleGirlsOnSchoolRollAtTimeOfTests,
        CurrentTimePeriod
    )]
    public string? GELIG_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfEligiblePupilsWithSENEHCPlanOrSENSupport,
        CurrentTimePeriod
    )]
    public string? PSENELE_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.PercentageOfEligiblePupilsWithSENSupport,
        CurrentTimePeriod
    )]
    public string? PSENELK_Est_Current_Pct_Coded { get; set; }

    [EESInput(
        Identifiers.NumberOfEligiblePupilsWithEnglishAsAdditionalLanguageEAL,
        CurrentTimePeriod
    )]
    public string? TEALGRP2_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.NumberOfEligiblePupils2023,
        CurrentTimePeriod
    )]
    public string? TELIG_Est_Current_Num_Coded { get; set; }

    [EESInput(
    Identifiers.TotalNumberOfPupilsIncludingPartTimePupils,
        CurrentTimePeriod
    )]
    public string? TPUPYEAR_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.NumberOfKeyStage2DisadvantagedPupils,
        CurrentTimePeriod
    )]
    public string? TFSM6CLA1A_Est_Current_Num_Coded { get; set; }

    [EESInput(
        Identifiers.NumberOfEligiblePupilsClassifiedAsNonMobile,
        CurrentTimePeriod
    )]
    public string? TMOBN_Est_Current_Num_Coded { get; set; }

}
