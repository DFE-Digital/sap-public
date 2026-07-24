using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels.Destinations;

namespace SAPPub.Core.Services;

public class DestinationsService(
    IEstablishmentService establishmentService, 
    IKS4DestinationsRepository kS4DestinationsRepository,
    IKS5DestinationsRepository kS5DestinationsRepository) : IDestinationsService
{
    public async Task<KS4DestinationsDetails> GetKS4DestinationsDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
        var laCode = establishment?.LAId ?? string.Empty;

        if (string.IsNullOrWhiteSpace(establishment?.URN))
        {
            return CreateEmpty(establishment?.URN);
        }
            
        // Run independent calls concurrently (LA depends on LAId being available)
        var establishmentDestinationsTask = kS4DestinationsRepository.GetEstablishmentDestinationsAsync(urn, ct);
        var englandDestinationsTask = kS4DestinationsRepository.GetEnglandDestinationsAsync(ct);
        var laDestinationsTask = kS4DestinationsRepository.GetLADestinationsAsync(laCode, ct);

        await Task.WhenAll(establishmentDestinationsTask, laDestinationsTask, englandDestinationsTask);

        ct.ThrowIfCancellationRequested();

        var establishmentDestinations = await establishmentDestinationsTask;
        var lADestinations = await laDestinationsTask;
        var englandDestinations = await englandDestinationsTask;

        // If your establishment destinations service returns nullable, handle it here.
        establishmentDestinations ??= new KS4EstablishmentDestinations();

        return new KS4DestinationsDetails
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LocalAuthorityName = establishment.LAName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            SchoolAll = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.AllDest_Tot_Est_Current_Pct,
                PreviousYear = establishmentDestinations.AllDest_Tot_Est_Previous_Pct,
                TwoYearsAgo = establishmentDestinations.AllDest_Tot_Est_Previous2_Pct,
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.AllDest_Tot_LA_Current_Pct,
                PreviousYear = lADestinations.AllDest_Tot_LA_Previous_Pct,
                TwoYearsAgo = lADestinations.AllDest_Tot_LA_Previous2_Pct,
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.AllDest_Tot_Eng_Current_Pct,
                PreviousYear = englandDestinations.AllDest_Tot_Eng_Previous_Pct,
                TwoYearsAgo = englandDestinations.AllDest_Tot_Eng_Previous2_Pct,
            },

            SchoolEducation = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.Education_Tot_Est_Current_Pct
            },
            LocalAuthorityEducation = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.Education_Tot_LA_Current_Pct
            },
            EnglandEducation = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.Education_Tot_Eng_Current_Pct
            },

            SchoolEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.Employment_Tot_Est_Current_Pct
            },
            LocalAuthorityEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.Employment_Tot_LA_Current_Pct
            },
            EnglandEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.Employment_Tot_Eng_Current_Pct
            },

            SchoolApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.Apprentice_Tot_Est_Current_Pct
            },
            LocalAuthorityApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.Apprentice_Tot_LA_Current_Pct
            },
            EnglandApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.Apprentice_Tot_Eng_Current_Pct
            },
        };
    }

    public async Task<KS5DestinationsDetails> GetKS5DestinationsDetailsAsync(string urn, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
        var laCode = establishment.LAId ?? string.Empty;

        if (string.IsNullOrWhiteSpace(establishment.URN))
        {
            return CreateEmptyKS5Destinations(establishment.URN);
        }

        // Run independent calls concurrently (LA depends on LAId being available)
        var establishmentDestinationsTask = kS5DestinationsRepository.GetEstablishmentDestinationsAsync(urn, ct);
        var englandDestinationsTask = kS5DestinationsRepository.GetEnglandDestinationsAsync(ct);
        var laDestinationsTask = kS5DestinationsRepository.GetLADestinationsAsync(laCode, ct);

        await Task.WhenAll(establishmentDestinationsTask, laDestinationsTask, englandDestinationsTask);

        var establishmentDestinations = await establishmentDestinationsTask;
        var lADestinations = await laDestinationsTask;
        var englandDestinations = await englandDestinationsTask;

        // If your establishment destinations service returns nullable, handle it here.
        establishmentDestinations ??= new KS5EstablishmentDestinations();

        return new KS5DestinationsDetails
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LocalAuthorityName = establishment.LAName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            EstablishmentTotalCohortFor = establishmentDestinations.TOT_COHORT_Est_Current_Num,
            EstablishmentTotalOverall = establishmentDestinations.TOT_OVERALLPER_Est_Current_Pct,
            LATotalOverall = lADestinations.TOT_OVERALLPER_LA_Current_Num,
            EnglandOverall = englandDestinations.TOT_OVERALLPER_Eng_Current_Pct
        };
    }

    private static KS5DestinationsDetails CreateEmptyKS5Destinations(string urn)
    {
        return new KS5DestinationsDetails
        {
            Urn = urn,
            SchoolName = string.Empty,
            LocalAuthorityName = string.Empty,
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = false
        };
    }

    private static KS4DestinationsDetails CreateEmpty(string? urn)
    {
        static RelativeYearValues<double?> EmptyYears() => new()
        {
            CurrentYear = null,
            PreviousYear = null,
            TwoYearsAgo = null
        };

        return new KS4DestinationsDetails
        {
            Urn = urn!,
            SchoolName = string.Empty,
            LocalAuthorityName = string.Empty,

            SchoolAll = EmptyYears(),
            LocalAuthorityAll = EmptyYears(),
            EnglandAll = EmptyYears(),

            SchoolEducation = EmptyYears(),
            LocalAuthorityEducation = EmptyYears(),
            EnglandEducation = EmptyYears(),

            SchoolEmployment = EmptyYears(),
            LocalAuthorityEmployment = EmptyYears(),
            EnglandEmployment = EmptyYears(),

            SchoolApprentice = EmptyYears(),
            LocalAuthorityApprentice = EmptyYears(),
            EnglandApprentice = EmptyYears(),

            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = false
        };
    }
}
