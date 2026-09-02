// ----------------------------
// EnglishAndMathsResultsService.cs
// ----------------------------
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

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
        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);

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
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,

            EstablishmentAll = Entities.KS4.Performance.EstablishmentPerformance.AllEnglishAndMaths(establishmentPerformance, selectedGrade),
            LocalAuthorityAll = Entities.KS4.Performance.LAPerformance.AllEnglishAndMaths(laPerformance, selectedGrade),
            EnglandAll = Entities.KS4.Performance.EnglandPerformance.AllEnglishAndMaths(englandPerformance, selectedGrade),

            EstablishmentBoys = Entities.KS4.Performance.EstablishmentPerformance.BoysEnglishAndMathsPerformance(establishmentPerformance, selectedGrade),
            LocalAuthorityBoys = Entities.KS4.Performance.LAPerformance.BoysEnglishAndMaths(laPerformance, selectedGrade),
            EnglandBoys = Entities.KS4.Performance.EnglandPerformance.BoysEnglishAndMaths(englandPerformance, selectedGrade),

            EstablishmentGirls = Entities.KS4.Performance.EstablishmentPerformance.GirlsEnglishAndMathsPerformance(establishmentPerformance, selectedGrade),
            LocalAuthorityGirls = Entities.KS4.Performance.LAPerformance.GirlsEnglishAndMaths(laPerformance, selectedGrade),
            EnglandGirls = Entities.KS4.Performance.EnglandPerformance.GirlsEnglishAndMaths(englandPerformance, selectedGrade),

            EstablishmentDisadvantaged = Entities.KS4.Performance.EstablishmentPerformance.DisadvantagedEnglishAndMathsPerformance(establishmentPerformance, selectedGrade),
            LocalAuthorityDisadvantaged = Entities.KS4.Performance.LAPerformance.DisadvantagedEnglishAndMaths(laPerformance, selectedGrade),
            EnglandDisadvantaged = Entities.KS4.Performance.EnglandPerformance.DisadvantagedEnglishAndMaths(englandPerformance, selectedGrade),

            LocalAuthorityNonDisadvantaged = Entities.KS4.Performance.LAPerformance.NonDisadvantagedEnglishAndMaths(laPerformance, selectedGrade),
            EnglandNonDisadvantaged = Entities.KS4.Performance.EnglandPerformance.NonDisadvantagedEnglishAndMaths(englandPerformance, selectedGrade)
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

            EstablishmentDisadvantaged = EmptyYears(),
            LocalAuthorityDisadvantaged = EmptyYears(),
            EnglandDisadvantaged = EmptyYears(),
            LocalAuthorityNonDisadvantaged = EmptyYears(),
            EnglandNonDisadvantaged = EmptyYears(),

            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = false
        };
    }
}
