using EES.API.POC.KS2Information.v1_x;
using SAPPub.Infrastructure.EES.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SAPPub.Infrastructure.EES.Entities.DTOs.KS2;

[ExcludeFromCodeCoverage]
public class KS2LAInformationDTO
{
    private const string CurrentTimePeriod = EESYearConstants.Year202425;
    private const string PreviousTimePeriod = EESYearConstants.Year202324;
    private const string Previous2TimePeriod = EESYearConstants.Year202223;

    public string Id { get; set; } = string.Empty;


    [EESInput(
        KS2InformationLocalAuthorityNational.Identifiers.NumberOfKeyStage2DisadvantagedPupils,
        CurrentTimePeriod,
        KS2InformationLocalAuthorityNational.Filters.EstablishmentTypeGroup.AllStateFunded
    )]
    public string? TFSM6CLA1A_LA_Current_Num_Coded { get; set; }

    [EESInput(
        KS2InformationLocalAuthorityNational.Identifiers.NumberOfKeyStage2PupilsWhoAreNotDisadvantaged,
        CurrentTimePeriod,
        KS2InformationLocalAuthorityNational.Filters.EstablishmentTypeGroup.AllStateFunded
    )]
    public string? TNOTFSM6CLA1A_LA_Current_Num_Coded { get; set; }

    [EESInput(
        KS2InformationLocalAuthorityNational.Identifiers.NumberOfEligiblePupils2023,
        CurrentTimePeriod,
        KS2InformationLocalAuthorityNational.Filters.EstablishmentTypeGroup.AllStateFunded
    )]
    public string? TELIG_LA_Current_Num_Coded { get; set; }

    [EESInput(
        KS2InformationLocalAuthorityNational.Identifiers.TotalNumberOfPupilsIncludingPartTimePupils,
        CurrentTimePeriod,
        KS2InformationLocalAuthorityNational.Filters.EstablishmentTypeGroup.AllStateFunded
    )]
    public string? TPUPYEAR_LA_Current_Num_Coded { get; set; }

}
