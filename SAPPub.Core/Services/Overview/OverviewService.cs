using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Overview;
using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Core.ServiceModels.Overview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPPub.Core.Helpers;

namespace SAPPub.Core.Services.Overview
{
    public class OverviewService(IOverviewRepository overviewRepository) : IOverviewService
    {
        public async Task<OverviewModel?> GetOverviewAsync(
            string urn,
            CancellationToken ct = default)
        {
            var overview =
                await overviewRepository.GetOverviewAsync(urn, ct);

            if (overview?.Establishment is null)
                return null;

            return new OverviewModel
            {
                Urn = overview.Establishment.URN,
                SchoolName = overview.Establishment.EstablishmentName,
                LocalAuthorityName = overview.Establishment.LAName,
                PhaseOfEducation = overview.Establishment.PhaseOfEducationName,
                AgeRangeLow = overview.Establishment.AgeRangeLow,
                AgeRangeHigh = overview.Establishment.AgeRangeHigh,
                NumberOfPupils = overview.Establishment.TotalPupils,
                SenProvision = overview.Establishment.SenTypes,
                Phone = overview.Establishment.TelephoneNum,
                Website = overview.Establishment.Website,
                Address = TextHelpers.ConcatListToString(
                [
                    overview.Establishment.AddressStreet,
                    overview.Establishment.AddressLocality,
                    overview.Establishment.AddressAddress3,
                    overview.Establishment.AddressTown,
                    overview.Establishment.AddressPostcode
                ]),
                Easting = overview.Establishment.Easting,
                Northing = overview.Establishment.Northing,

                IsKS2 = overview.Establishment.IsKS2,
                IsKS4 = overview.Establishment.IsKS4,
                IsKS5 = overview.Establishment.IsKS5,

                Attainment8 = overview.KS4Performance?.Attainment8_Tot_Est_Current_Num_Coded,
                Attainment8LA = overview.KS4LAPerformance?.Attainment8_Tot_LA_Current_Num_Coded,
                Attainment8England = overview.KS4EnglandPerformance?.Attainment8_Tot_Eng_Current_Num_Coded,
                EnglishAndMathsGrade5Establishment =  overview.KS4Performance?.EngMaths59_Tot_Est_Current_Pct_Coded,
                EnglishAndMathsGrade5LA = overview.KS4LAPerformance?.EngMaths59_Tot_LA_Current_Pct_Coded,
                EnglishAndMathsGrade5England = overview.KS4EnglandPerformance?.EngMaths59_Tot_Eng_Current_Pct_Coded,
                MoreThanOneForeignLanguage = overview.KS4Performance?.More1FL_Tot_Est_Current_Pct_Coded,
                DestinationsEstablishment = overview.Destinations?.AllDest_Tot_Est_Current_Pct_Coded,
                DestinationsLA = overview.LADestinations?.AllDest_Tot_LA_Current_Pct_Coded,
                DestinationsEngland = overview.EnglandDestinations?.AllDest_Tot_Eng_Current_Pct_Coded,
                ReadingWritingMathsExpectedEstablishment =  overview.KS2Performance?.PTRWM_EXP_Est_Current_Pct_Coded,
                ReadingWritingMathsExpectedLA =  overview.KS2LAPerformance?.PTRWM_EXP_LA_Current_Pct_Coded,
                ReadingWritingMathsExpectedEngland = overview.KS2EnglandPerformance?.PTRWM_EXP_Eng_Current_Pct_Coded,
                ReadingWritingMathsHigherEstablishment = overview.KS2Performance?.PTRWM_HIGH_Est_Current_Pct_Coded,
                ReadingWritingMathsHigherLA = overview.KS2LAPerformance?.PTRWM_HIGH_LA_Current_Pct_Coded,
                ReadingWritingMathsHigherEngland = overview.KS2EnglandPerformance?.PTRWM_HIGH_Eng_Current_Pct_Coded
            };
        }
    }
}
