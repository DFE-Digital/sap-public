using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories.Overview;
using SAPPub.Infrastructure.Repositories.Generic;
using SAPPub.Core.Entities.Overview;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories.Overview;

// This is a little messy, but the other obvious options are to route back to the services (not supported), or read the files directly (could 
public sealed class OverviewSimpleRepository(
    IGenericRepository<Establishment> establishmentRepository,
    IGenericRepository<EstablishmentPerformance> establishmentPerformanceRepository,
    IGenericRepository<LAPerformance> laPerformanceRepository,
    IGenericRepository<EnglandPerformance> englandPerformanceRepository,
    IGenericRepository<KS4EstablishmentDestinations> establishmentDestinationsRepository,
    IGenericRepository<KS4LADestinations> laDestinationsRepository,
    IGenericRepository<KS4EnglandDestinations> englandDestinationsRepository,
    IGenericRepository<KS2EstablishmentPerformance> ks2EstablishmentPerformanceRepository,
    IGenericRepository<KS2LAPerformance> ks2LAPerformanceRepository,
    IGenericRepository<KS2EnglandPerformance> ks2EnglandPerformanceRepository,
    IGenericRepository<TechnicalSubjectEntry> technicalSubjectEntryRepository
    ) : IOverviewRepository
{
    private readonly IGenericRepository<Establishment> _establishmentRepository = 
        establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));

    private readonly IGenericRepository<EstablishmentPerformance> _establishmentPerformanceRepository =
        establishmentPerformanceRepository ?? throw new ArgumentNullException(nameof(establishmentPerformanceRepository));

    private readonly IGenericRepository<LAPerformance> _laPerformanceRepository =
        laPerformanceRepository ?? throw new ArgumentNullException(nameof(laPerformanceRepository));
    private readonly IGenericRepository<EnglandPerformance> _englandPerformanceRepository =
        englandPerformanceRepository ?? throw new ArgumentNullException(nameof(englandPerformanceRepository));

    private readonly IGenericRepository<KS4EstablishmentDestinations> _establishmentDestinationsRepository =
        establishmentDestinationsRepository ?? throw new ArgumentNullException(nameof(establishmentDestinationsRepository));

    private readonly IGenericRepository<KS4LADestinations> _laDestinationsRepository =
        laDestinationsRepository ?? throw new ArgumentNullException(nameof(laDestinationsRepository));

    private readonly IGenericRepository<KS4EnglandDestinations> _englandDestinationsRepository =
        englandDestinationsRepository ?? throw new ArgumentNullException(nameof(englandDestinationsRepository));

    private readonly IGenericRepository<KS2EstablishmentPerformance> _ks2EstablishmentPerformanceRepository =
        ks2EstablishmentPerformanceRepository ?? throw new ArgumentNullException(nameof(ks2EstablishmentPerformanceRepository));

    private readonly IGenericRepository<KS2LAPerformance> _ks2LAPerformanceRepository =
        ks2LAPerformanceRepository ?? throw new ArgumentNullException(nameof(ks2LAPerformanceRepository));

    private readonly IGenericRepository<KS2EnglandPerformance> _ks2EnglandPerformanceRepository =
        ks2EnglandPerformanceRepository ?? throw new ArgumentNullException(nameof(ks2EnglandPerformanceRepository));

    private readonly IGenericRepository<TechnicalSubjectEntry> _technicalSubjectEntryRepository =
        technicalSubjectEntryRepository ?? throw new ArgumentNullException(nameof(technicalSubjectEntryRepository));



    public async Task<Core.Entities.Overview.Overview?> GetOverviewAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return null;

        //const string sql = """
        //    -- 1. Establishment
        //    SELECT
        //        e."URN",
        //        e."EstablishmentName",
        //        e."LAId",
        //        e."LAName",
        //        e."PhaseOfEducationName",
        //        e."AgeRangeLow",
        //        e."AgeRangeHigh",
        //        e."TotalPupils",
        //        e."SenTypes",
        //        e."TelephoneNum",
        //        e."Website",
        //        e."AddressStreet",
        //        e."AddressLocality",
        //        e."AddressAddress3",
        //        e."AddressTown",
        //        e."AddressPostcode",
        //        e."Easting",
        //        e."Northing",
        //        e."ISKS2",
        //        e."ISKS4",
        //        e."ISKS5"
        //    FROM public.v_establishment e
        //    WHERE e."URN" = @Urn;

        //    -- 2. KS4 establishment performance
        //    SELECT
        //        p."Id",
        //        p."Attainment8_Tot_Est_Current_Num_Coded",
        //        p."EngMaths59_Tot_Est_Current_Pct_Coded",
        //        p."More1FL_Tot_Est_Current_Pct_Coded",
        //        p."GCSESubjectEnteredSum_Est_Current_Num_Coded",
        //        p."TechSubjectEnteredSum_Est_Current_Num_Coded"
        //    FROM public.v_establishment e
        //    LEFT JOIN public.v_establishment_performance p
        //        ON p."Id" = e."URN"
        //    WHERE e."URN" = @Urn;

        //    -- 3. KS4 local authority performance
        //    SELECT
        //        la."Id",
        //        la."Attainment8_Tot_LA_Current_Num_Coded",
        //        la."EngMaths59_Tot_LA_Current_Pct_Coded"
        //    FROM public.v_establishment e
        //    LEFT JOIN public.v_la_performance la
        //        ON la."Id" = e."LAId"
        //    WHERE e."URN" = @Urn;

        //    -- 4. KS4 England performance
        //    SELECT
        //        eng."Id",
        //        eng."Attainment8_Tot_Eng_Current_Num_Coded",
        //        eng."EngMaths59_Tot_Eng_Current_Pct_Coded"
        //    FROM public.v_england_performance eng
        //    WHERE eng."Id" = 'National';

        //    -- 5. Establishment destinations
        //    SELECT
        //        d."Id",
        //        d."AllDest_Tot_Est_Current_Pct_Coded"
        //    FROM public.v_establishment e
        //    LEFT JOIN public.v_establishment_destinations d
        //        ON d."Id" = e."URN"
        //    WHERE e."URN" = @Urn;

        //    -- 6. Local authority destinations
        //    SELECT
        //        la."Id",
        //        la."AllDest_Tot_LA_Current_Pct_Coded"
        //    FROM public.v_establishment e
        //    LEFT JOIN public.v_la_destinations la
        //        ON la."Id" = e."LAId"
        //    WHERE e."URN" = @Urn;

        //    -- 7. England destinations
        //    SELECT
        //        eng."Id",
        //        eng."AllDest_Tot_Eng_Current_Pct_Coded"
        //    FROM public.v_england_destinations eng
        //    WHERE eng."Id" = 'National';

        //    -- 8. KS2 establishment performance
        //    SELECT
        //        p."Id",
        //        p."PTRWM_EXP_Est_Current_Pct_Coded",
        //        p."PTRWM_HIGH_Est_Current_Pct_Coded"
        //    FROM public.v_establishment e
        //    LEFT JOIN public.v_establishment_ks2_attainment p
        //        ON p."Id" = e."URN"
        //    WHERE e."URN" = @Urn;

        //    -- 9. KS2 local authority performance
        //    SELECT
        //        la."Id",
        //        la."PTRWM_EXP_LA_Current_Pct_Coded",
        //        la."PTRWM_HIGH_LA_Current_Pct_Coded"
        //    FROM public.v_establishment e
        //    LEFT JOIN public.v_la_ks2_attainment la
        //        ON la."Id" = e."LAId"
        //    WHERE e."URN" = @Urn;

        //    -- 10. KS2 England performance
        //    SELECT
        //        eng."Id",
        //        eng."PTRWM_EXP_Eng_Current_Pct_Coded",
        //        eng."PTRWM_HIGH_Eng_Current_Pct_Coded"
        //    FROM public.v_england_ks2_attainment eng
        //    WHERE eng."Id" = 'National';

        //    -- 11. Top technical subjects
        //    SELECT
        //        s."SubjectName",
        //        s."PercentageEntering"
        //    FROM public.v_establishment_top3_technical_subject_entries s
        //    WHERE s."URN" = @Urn
        //    ORDER BY
        //        s."PercentageEntering" DESC NULLS LAST,
        //        s."SubjectName";
        //    """;

        //await using var connection = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);

        //var command = new DapperCommandBuilder()
        //    .WithCommandText(sql)
        //    .WithParameters(new { Urn = urn })
        //    .Build(ct);

        //using var result = await connection.QueryMultipleAsync(command).ConfigureAwait(false); //only hit database once

        //var establishment = await result.ReadSingleOrDefaultAsync<Establishment>();



        //var ks4Performance = await result.ReadSingleOrDefaultAsync<EstablishmentPerformance>();

        //var ks4LAPerformance = await result.ReadSingleOrDefaultAsync<LAPerformance>();

        //var ks4EnglandPerformance = await result.ReadSingleOrDefaultAsync<EnglandPerformance>();

        //var destinations = await result.ReadSingleOrDefaultAsync<KS4EstablishmentDestinations>();

        //var laDestinations = await result.ReadSingleOrDefaultAsync<KS4LADestinations>();

        //var englandDestinations = await result.ReadSingleOrDefaultAsync<KS4EnglandDestinations>();

        //var ks2Performance = await result.ReadSingleOrDefaultAsync<KS2EstablishmentPerformance>();

        //var ks2LAPerformance = await result.ReadSingleOrDefaultAsync<KS2LAPerformance>();

        //var ks2EnglandPerformance = await result.ReadSingleOrDefaultAsync<KS2EnglandPerformance>();

        //var topTechnicalSubjects = (await result.ReadAsync<TechnicalSubjectEntry>()).ToList();

        var establishment = await _establishmentRepository.ReadAsync(urn, ct).ConfigureAwait(false);

        if (establishment is null)
            throw new NotFoundException($"Establishment not found with URN: {urn}");

        var ks4Performance = await _establishmentPerformanceRepository.ReadAsync(urn, ct).ConfigureAwait(false);

        var ks4LAPerformance = await _laPerformanceRepository.ReadAsync(establishment.LAId, ct).ConfigureAwait(false);

        var ks4EnglandPerformance = await _englandPerformanceRepository.ReadAsync("National", ct).ConfigureAwait(false);

        var destinations = await _establishmentDestinationsRepository.ReadAsync(urn, ct).ConfigureAwait(false);

        var laDestinations = await _laDestinationsRepository.ReadAsync(establishment.LAId, ct).ConfigureAwait(false);

        var englandDestinations = await _englandDestinationsRepository.ReadAsync("National", ct).ConfigureAwait(false);
        
        var ks2Performance = await _ks2EstablishmentPerformanceRepository.ReadAsync(urn, ct).ConfigureAwait(false);

        var ks2LAPerformance = await _ks2LAPerformanceRepository.ReadAsync(establishment.LAId, ct).ConfigureAwait(false);

        var ks2EnglandPerformance = await _ks2EnglandPerformanceRepository.ReadAsync("National", ct).ConfigureAwait(false);

        var topTechnicalSubjects = (await _technicalSubjectEntryRepository.ReadManyAsync(new { Urn = urn }, ct).ConfigureAwait(false)).ToList();


        return new Core.Entities.Overview.Overview
        {
            Establishment = establishment,

            KS4Performance = ks4Performance,
            KS4LAPerformance = ks4LAPerformance,
            KS4EnglandPerformance = ks4EnglandPerformance,

            Destinations = destinations,
            LADestinations = laDestinations,
            EnglandDestinations = englandDestinations,

            KS2Performance = ks2Performance,
            KS2LAPerformance = ks2LAPerformance,
            KS2EnglandPerformance = ks2EnglandPerformance,

            TopTechnicalSubjects = topTechnicalSubjects
        };
    }
}