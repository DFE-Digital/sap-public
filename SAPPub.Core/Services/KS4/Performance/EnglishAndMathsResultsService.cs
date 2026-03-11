// ----------------------------
// EnglishAndMathsResultsService.cs
// ----------------------------
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.KS4.Performance;

public sealed class EnglishAndMathsResultsService(
    IEstablishmentService establishmentService,
    IEstablishmentPerformanceService establishmentPerformanceService,
    ILAPerformanceService lAPerformanceService,
    IEnglandPerformanceService englandPerformanceService
) : IAcademicPerformanceEnglishAndMathsResultsService
{
    public async Task<EnglishAndMathsResultsModel> GetEnglishAndMathsResultsAsync(
        string urn,
        int selectedGrade,
        CancellationToken ct = default)
    {
        // Need establishment first to get LAId/LAName (and to check if URN is valid)
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishment.URN))
            return CreateEmpty(urn);

        // Now we can run the remaining calls concurrently
        var establishmentPerformanceTask = establishmentPerformanceService.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = englandPerformanceService.GetEnglandPerformanceAsync(ct);

        var laId = establishment.LAId ?? string.Empty;
        var laPerformanceTask = lAPerformanceService.GetLAPerformanceAsync(laId, ct);

        await Task.WhenAll(establishmentPerformanceTask, laPerformanceTask, englandPerformanceTask);

        var establishmentPerformance = await establishmentPerformanceTask;
        var laPerformance = await laPerformanceTask;
        var englandPerformance = await englandPerformanceTask;

        return new EnglishAndMathsResultsModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LAName = establishment.LAName,

            EstablishmentAll = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => establishmentPerformance.EngMaths49_Tot_Est_Current_Pct,
                    5 => establishmentPerformance.EngMaths59_Tot_Est_Current_Pct,
                    _ => null
                },
                PreviousYear = selectedGrade switch
                {
                    4 => establishmentPerformance.EngMaths49_Tot_Est_Previous_Pct,
                    5 => establishmentPerformance.EngMaths59_Tot_Est_Previous_Pct,
                    _ => null
                },
                TwoYearsAgo = selectedGrade switch
                {
                    4 => establishmentPerformance.EngMaths49_Tot_Est_Previous2_Pct,
                    5 => establishmentPerformance.EngMaths59_Tot_Est_Previous2_Pct,
                    _ => null
                },
            },

            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => laPerformance.EngMaths49_Tot_LA_Current_Pct,
                    5 => laPerformance.EngMaths59_Tot_LA_Current_Pct,
                    _ => null
                },
                PreviousYear = selectedGrade switch
                {
                    4 => laPerformance.EngMaths49_Tot_LA_Previous_Pct,
                    5 => laPerformance.EngMaths59_Tot_LA_Previous_Pct,
                    _ => null
                },
                TwoYearsAgo = selectedGrade switch
                {
                    4 => laPerformance.EngMaths49_Tot_LA_Previous2_Pct,
                    5 => laPerformance.EngMaths59_Tot_LA_Previous2_Pct,
                    _ => null
                },
            },

            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Tot_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Tot_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Tot_Eng_Previous_Pct,
                    5 => englandPerformance.EngMaths59_Tot_Eng_Previous_Pct,
                    _ => null
                },
                TwoYearsAgo = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Tot_Eng_Previous2_Pct,
                    5 => englandPerformance.EngMaths59_Tot_Eng_Previous2_Pct,
                    _ => null
                },
            },

            EstablishmentBoys = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => establishmentPerformance.EngMaths49_Boy_Est_Current_Pct,
                    5 => establishmentPerformance.EngMaths59_Boy_Est_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            },

            LocalAuthorityBoys = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => laPerformance.EngMaths49_Boy_LA_Current_Pct,
                    5 => laPerformance.EngMaths59_Boy_LA_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            },

            EnglandBoys = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Boy_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Boy_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            },

            EstablishmentGirls = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => establishmentPerformance.EngMaths49_Grl_Est_Current_Pct,
                    5 => establishmentPerformance.EngMaths59_Grl_Est_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            },

            LocalAuthorityGirls = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => laPerformance.EngMaths49_Grl_LA_Current_Pct,
                    5 => laPerformance.EngMaths59_Grl_LA_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            },

            EnglandGirls = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch
                {
                    4 => englandPerformance.EngMaths49_Grl_Eng_Current_Pct,
                    5 => englandPerformance.EngMaths59_Grl_Eng_Current_Pct,
                    _ => null
                },
                PreviousYear = null,
                TwoYearsAgo = null
            },
        };
    }

    private static EnglishAndMathsResultsModel CreateEmpty(string urn)
    {
        static RelativeYearValues<double?> EmptyYears() => new()
        {
            CurrentYear = null,
            PreviousYear = null,
            TwoYearsAgo = null
        };

        return new EnglishAndMathsResultsModel
        {
            Urn = urn,
            SchoolName = string.Empty,
            LAName = null,

            EstablishmentAll = EmptyYears(),
            LocalAuthorityAll = EmptyYears(),
            EnglandAll = EmptyYears(),

            EstablishmentBoys = EmptyYears(),
            LocalAuthorityBoys = EmptyYears(),
            EnglandBoys = EmptyYears(),

            EstablishmentGirls = EmptyYears(),
            LocalAuthorityGirls = EmptyYears(),
            EnglandGirls = EmptyYears(),
        };
    }
}
