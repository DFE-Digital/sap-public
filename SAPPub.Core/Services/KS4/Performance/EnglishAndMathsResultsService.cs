// ----------------------------
// EnglishAndMathsResultsService.cs
// ----------------------------
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;

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

            EstablishmentAll = AllEnglishAndMaths(establishmentPerformance, selectedGrade),
            LocalAuthorityAll = AllEnglishAndMaths(laPerformance, selectedGrade),
            EnglandAll = AllEnglishAndMaths(englandPerformance, selectedGrade),

            EstablishmentBoys = BoysEnglishAndMathsPerformance(establishmentPerformance, selectedGrade),
            LocalAuthorityBoys = BoysEnglishAndMaths(laPerformance, selectedGrade),
            EnglandBoys = BoysEnglishAndMaths(englandPerformance, selectedGrade),

            EstablishmentGirls = GirlsEnglishAndMathsPerformance(establishmentPerformance, selectedGrade),
            LocalAuthorityGirls = GirlsEnglishAndMaths(laPerformance, selectedGrade),
            EnglandGirls = GirlsEnglishAndMaths(englandPerformance, selectedGrade),

            EstablishmentDisadvantaged = DisadvantagedEnglishAndMathsPerformance(establishmentPerformance, selectedGrade),
            LocalAuthorityDisadvantaged = DisadvantagedEnglishAndMaths(laPerformance, selectedGrade),
            EnglandDisadvantaged = DisadvantagedEnglishAndMaths(englandPerformance, selectedGrade),

            LocalAuthorityNonDisadvantaged = NonDisadvantagedEnglishAndMaths(laPerformance, selectedGrade),
            EnglandNonDisadvantaged = NonDisadvantagedEnglishAndMaths(englandPerformance, selectedGrade)
        };
    }

    private static EnglishAndMathsResultsModel CreateEmpty(string urn)
    {
        static RelativeYearValues<T> EmptyYears<T>() where T : new() =>
            new()
            {
                CurrentYear = new T(),
                PreviousYear = new T(),
                TwoYearsAgo = new T()
            };

        return new EnglishAndMathsResultsModel
        {
            Urn = urn,
            SchoolName = string.Empty,
            LAName = null,

            EstablishmentAll = EmptyYears<double?>(),
            LocalAuthorityAll = EmptyYears<double?>(),
            EnglandAll = EmptyYears<double?>(),

            EstablishmentBoys = EmptyYears<double?>(),
            LocalAuthorityBoys = EmptyYears<double?>(),
            EnglandBoys = EmptyYears<double?>(),

            EstablishmentGirls = EmptyYears<double?>(),
            LocalAuthorityGirls = EmptyYears<double?>(),
            EnglandGirls = EmptyYears<double?>(),

            EstablishmentDisadvantaged = EmptyYears<CodedDouble>(),
            LocalAuthorityDisadvantaged = EmptyYears<CodedDouble>(),
            EnglandDisadvantaged = EmptyYears<CodedDouble>(),
            LocalAuthorityNonDisadvantaged = EmptyYears<CodedDouble>(),
            EnglandNonDisadvantaged = EmptyYears<CodedDouble>(),

            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = false
        };
    }

    private RelativeYearValues<double?> AllEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
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

    public static RelativeYearValues<CodedDouble> DisadvantagedEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = selectedGrade switch
            {
                4 => englandPerformance.EngMaths49_Dis_Eng_Current_Pct_Coded,
                5 => englandPerformance.EngMaths59_Dis_Eng_Current_Pct_Coded,
                7 => englandPerformance.EngMaths79_Dis_Eng_Current_Pct_Coded,
                _ => new CodedDouble()
            },
            PreviousYear = new CodedDouble(),
            TwoYearsAgo = new CodedDouble()
        };
    }

    public static RelativeYearValues<CodedDouble> NonDisadvantagedEnglishAndMaths(EnglandPerformance englandPerformance, int selectedGrade)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = selectedGrade switch
            {
                4 => englandPerformance.EngMaths49_NDi_Eng_Current_Pct_Coded,
                5 => englandPerformance.EngMaths59_NDi_Eng_Current_Pct_Coded,
                7 => englandPerformance.EngMaths79_NDi_Eng_Current_Pct_Coded,
                _ => new CodedDouble()
            },
            PreviousYear = new CodedDouble(),
            TwoYearsAgo = new CodedDouble()
        };
    }

    public static RelativeYearValues<double?> AllEnglishAndMaths(EstablishmentPerformance establishmentPerformance, int selectedGrade)
    {
        return new RelativeYearValues<double?>
        {
            CurrentYear = selectedGrade switch
            {
                4 => establishmentPerformance.EngMaths49_Tot_Est_Current_Pct,
                5 => establishmentPerformance.EngMaths59_Tot_Est_Current_Pct,
                7 => establishmentPerformance.EngMaths79_Tot_Est_Current_Pct,
                _ => null
            },
            PreviousYear = selectedGrade switch
            {
                4 => establishmentPerformance.EngMaths49_Tot_Est_Previous_Pct,
                5 => establishmentPerformance.EngMaths59_Tot_Est_Previous_Pct,
                7 => establishmentPerformance.EngMaths79_Tot_Est_Previous_Pct,
                _ => null
            },
            TwoYearsAgo = selectedGrade switch
            {
                4 => establishmentPerformance.EngMaths49_Tot_Est_Previous2_Pct,
                5 => establishmentPerformance.EngMaths59_Tot_Est_Previous2_Pct,
                7 => establishmentPerformance.EngMaths79_Tot_Est_Previous2_Pct,
                _ => null
            }
        };
    }

    public static RelativeYearValues<double?> GirlsEnglishAndMathsPerformance(EstablishmentPerformance establishmentPerformance, int selectedGrade)
    {
        return new RelativeYearValues<double?>
        {
            CurrentYear = selectedGrade switch
            {
                4 => establishmentPerformance.EngMaths49_Grl_Est_Current_Pct,
                5 => establishmentPerformance.EngMaths59_Grl_Est_Current_Pct,
                7 => establishmentPerformance.EngMaths79_Grl_Est_Current_Pct,
                _ => null
            },
            PreviousYear = null,
            TwoYearsAgo = null
        };
    }

    public static RelativeYearValues<double?> BoysEnglishAndMathsPerformance(EstablishmentPerformance establishmentPerformance, int selectedGrade)
    {
        return new RelativeYearValues<double?>
        {
            CurrentYear = selectedGrade switch
            {
                4 => establishmentPerformance.EngMaths49_Boy_Est_Current_Pct,
                5 => establishmentPerformance.EngMaths59_Boy_Est_Current_Pct,
                7 => establishmentPerformance.EngMaths79_Boy_Est_Current_Pct,
                _ => null
            },
            PreviousYear = null,
            TwoYearsAgo = null
        };
    }

    public static RelativeYearValues<CodedDouble> DisadvantagedEnglishAndMathsPerformance(EstablishmentPerformance establishmentPerformance, int selectedGrade)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = selectedGrade switch
            {
                4 => establishmentPerformance.EngMaths49_Dis_Est_Current_Pct_Coded,
                5 => establishmentPerformance.EngMaths59_Dis_Est_Current_Pct_Coded,
                7 => establishmentPerformance.EngMaths79_Dis_Est_Current_Pct_Coded,
                _ => new CodedDouble()
            },
            PreviousYear = new CodedDouble(),
            TwoYearsAgo = new CodedDouble()
        };
    }

    public static RelativeYearValues<double?> GirlsEnglishAndMaths(LAPerformance laPerformance, int selectedGrade)
    {
        return new RelativeYearValues<double?>
        {
            CurrentYear = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_Grl_LA_Current_Pct,
                5 => laPerformance.EngMaths59_Grl_LA_Current_Pct,
                7 => laPerformance.EngMaths79_Grl_LA_Current_Pct,
                _ => null
            },
            PreviousYear = null,
            TwoYearsAgo = null
        };
    }

    public static RelativeYearValues<double?> BoysEnglishAndMaths(LAPerformance laPerformance, int selectedGrade)
    {
        return new RelativeYearValues<double?>
        {
            CurrentYear = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_Boy_LA_Current_Pct,
                5 => laPerformance.EngMaths59_Boy_LA_Current_Pct,
                7 => laPerformance.EngMaths79_Boy_LA_Current_Pct,
                _ => null
            },
            PreviousYear = null,
            TwoYearsAgo = null
        };
    }

    public static RelativeYearValues<CodedDouble> DisadvantagedEnglishAndMaths(LAPerformance laPerformance, int selectedGrade)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_Dis_LA_Current_Pct_Coded,
                5 => laPerformance.EngMaths59_Dis_LA_Current_Pct_Coded,
                7 => laPerformance.EngMaths79_Dis_LA_Current_Pct_Coded,
                _ => new CodedDouble()
            },
            PreviousYear = new CodedDouble(),
            TwoYearsAgo = new CodedDouble()
        };
    }

    public static RelativeYearValues<CodedDouble> NonDisadvantagedEnglishAndMaths(LAPerformance laPerformance, int selectedGrade)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_NDi_LA_Current_Pct_Coded,
                5 => laPerformance.EngMaths59_NDi_LA_Current_Pct_Coded,
                7 => laPerformance.EngMaths79_NDi_LA_Current_Pct_Coded,
                _ => new CodedDouble()
            },
            PreviousYear = new CodedDouble(),
            TwoYearsAgo = new CodedDouble()
        };
    }

    public static RelativeYearValues<double?> AllEnglishAndMaths(LAPerformance laPerformance, int selectedGrade)
    {
        return new RelativeYearValues<double?>
        {
            CurrentYear = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_Tot_LA_Current_Pct,
                5 => laPerformance.EngMaths59_Tot_LA_Current_Pct,
                7 => laPerformance.EngMaths79_Tot_LA_Current_Pct,
                _ => null
            },
            PreviousYear = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_Tot_LA_Previous_Pct,
                5 => laPerformance.EngMaths59_Tot_LA_Previous_Pct,
                7 => laPerformance.EngMaths79_Tot_LA_Previous_Pct,
                _ => null
            },
            TwoYearsAgo = selectedGrade switch
            {
                4 => laPerformance.EngMaths49_Tot_LA_Previous2_Pct,
                5 => laPerformance.EngMaths59_Tot_LA_Previous2_Pct,
                7 => laPerformance.EngMaths79_Tot_LA_Previous2_Pct,
                _ => null
            }
        };
    }
}
