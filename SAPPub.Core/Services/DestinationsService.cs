using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels.Destinations;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services;

public class DestinationsService(
    IEstablishmentService establishmentService, 
    IKS4DestinationsRepository kS4DestinationsRepository,
    IKS5DestinationsRepository kS5DestinationsRepository) : IDestinationsService
{
    public async Task<KS4DestinationsDetails> GetKS4DestinationsDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);
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
            SchoolAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.AllDest_Tot_Est_Current_Pct_Coded,
                PreviousYear = establishmentDestinations.AllDest_Tot_Est_Previous_Pct_Coded,
                TwoYearsAgo = establishmentDestinations.AllDest_Tot_Est_Previous2_Pct_Coded,
            },
            LocalAuthorityAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.AllDest_Tot_LA_Current_Pct_Coded,
                PreviousYear = lADestinations.AllDest_Tot_LA_Previous_Pct_Coded,
                TwoYearsAgo = lADestinations.AllDest_Tot_LA_Previous2_Pct_Coded,
            },
            EnglandAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.AllDest_Tot_Eng_Current_Pct_Coded,
                PreviousYear = englandDestinations.AllDest_Tot_Eng_Previous_Pct_Coded,
                TwoYearsAgo = englandDestinations.AllDest_Tot_Eng_Previous2_Pct_Coded,
            },
            SchoolDisadvantagedAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.AllDest_Dis_Est_Current_Pct_Coded
            },
            LocalAuthorityDisadvantagedAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.AllDest_Dis_LA_Current_Pct_Coded
            },
            EnglandDisadvantagedAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.AllDest_Dis_Eng_Current_Pct_Coded
            },
            LocalAuthorityNonDisadvantagedAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.AllDest_Ndis_LA_Current_Pct_Coded
            },
            EnglandNonDisadvantagedAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.AllDest_Ndis_Eng_Current_Pct_Coded
            },
            SchoolEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.Education_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.Education_Tot_LA_Current_Pct_Coded
            },
            EnglandEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.Education_Tot_Eng_Current_Pct_Coded
            },

            SchoolEmployment = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.Employment_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityEmployment = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.Employment_Tot_LA_Current_Pct_Coded
            },
            EnglandEmployment = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.Employment_Tot_Eng_Current_Pct_Coded
            },

            SchoolApprentice = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.Apprentice_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityApprentice = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.Apprentice_Tot_LA_Current_Pct_Coded
            },
            EnglandApprentice = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.Apprentice_Tot_Eng_Current_Pct_Coded
            },

            SchoolFurtherEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.FurtherEd_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityFurtherEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.FurtherEd_Tot_LA_Current_Pct_Coded
            },
            EnglandFurtherEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.FurtherEd_Tot_Eng_Current_Pct_Coded
            },

            SchoolSchoolSixthForm = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.SchSixthForm_Tot_Est_Current_Pct_Coded
            },
            LocalAuthoritySchoolSixthForm = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.SchSixthForm_Tot_LA_Current_Pct_Coded
            },
            EnglandSchoolSixthForm = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.SchSixthForm_Tot_Eng_Current_Pct_Coded
            },

            SchoolCollegeSixthForm = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.ColSixthForm_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityCollegeSixthForm = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.ColSixthForm_Tot_LA_Current_Pct_Coded
            },
            EnglandCollegeSixthForm = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.ColSixthForm_Tot_Eng_Current_Pct_Coded
            },

            SchoolOtherEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.OtherEd_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityOtherEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.OtherEd_Tot_LA_Current_Pct_Coded
            },
            EnglandOtherEducation = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.OtherEd_Tot_Eng_Current_Pct_Coded
            },

            SchoolNotSustained = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.NotSus_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityNotSustained = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.NotSus_Tot_LA_Current_Pct_Coded
            },
            EnglandNotSustained = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.NotSus_Tot_Eng_Current_Pct_Coded
            },

            SchoolUnknown = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentDestinations.Unknown_Tot_Est_Current_Pct_Coded
            },
            LocalAuthorityUnknown = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = lADestinations.Unknown_Tot_LA_Current_Pct_Coded
            },
            EnglandUnknown = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandDestinations.Unknown_Tot_Eng_Current_Pct_Coded
            },
        };
    }

    public async Task<KS5DestinationsDetails> GetKS5DestinationsDetailsAsync(string urn, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);
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
            LATotalOverall = lADestinations.TOT_OVERALLPER_LA_Current_Pct,
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
        static RelativeYearValues<CodedDouble> EmptyYears() => new()
        {
            CurrentYear = CodedDouble.Empty,
            PreviousYear = CodedDouble.Empty,
            TwoYearsAgo = CodedDouble.Empty
        };

        return new KS4DestinationsDetails
        {
            Urn = urn!,
            SchoolName = string.Empty,
            LocalAuthorityName = string.Empty,

            SchoolAll = EmptyYears(),
            LocalAuthorityAll = EmptyYears(),
            EnglandAll = EmptyYears(),

            SchoolDisadvantagedAll = EmptyYears(),
            LocalAuthorityDisadvantagedAll = EmptyYears(),
            EnglandDisadvantagedAll = EmptyYears(),

            LocalAuthorityNonDisadvantagedAll = EmptyYears(),
            EnglandNonDisadvantagedAll = EmptyYears(),

            SchoolEducation = EmptyYears(),
            LocalAuthorityEducation = EmptyYears(),
            EnglandEducation = EmptyYears(),

            SchoolEmployment = EmptyYears(),
            LocalAuthorityEmployment = EmptyYears(),
            EnglandEmployment = EmptyYears(),

            SchoolApprentice = EmptyYears(),
            LocalAuthorityApprentice = EmptyYears(),
            EnglandApprentice = EmptyYears(),

            SchoolFurtherEducation = EmptyYears(),
            LocalAuthorityFurtherEducation = EmptyYears(),
            EnglandFurtherEducation = EmptyYears(),

            SchoolSchoolSixthForm = EmptyYears(),
            LocalAuthoritySchoolSixthForm = EmptyYears(),
            EnglandSchoolSixthForm = EmptyYears(),

            SchoolCollegeSixthForm = EmptyYears(),
            LocalAuthorityCollegeSixthForm = EmptyYears(),
            EnglandCollegeSixthForm = EmptyYears(),

            SchoolOtherEducation = EmptyYears(),
            LocalAuthorityOtherEducation = EmptyYears(),
            EnglandOtherEducation = EmptyYears(),

            SchoolNotSustained = EmptyYears(),
            LocalAuthorityNotSustained = EmptyYears(),
            EnglandNotSustained = EmptyYears(),

            SchoolUnknown = EmptyYears(),
            LocalAuthorityUnknown = EmptyYears(),
            EnglandUnknown = EmptyYears(),

            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = false
        };
    }
}
