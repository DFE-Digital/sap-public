using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories.Overview;
using SAPPub.Infrastructure.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories.Overview;

public class OverviewRepository(
    NpgsqlDataSource dataSource) : IOverviewRepository
{
    private readonly NpgsqlDataSource _dataSource =
        dataSource ?? throw new ArgumentNullException(nameof(dataSource));

    public async Task<Core.Entities.Overview.Overview?> GetOverviewAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return null;

        const string sql = """
            -- 1. Establishment
            SELECT
                e."URN",
                e."EstablishmentName",
                e."LAId",
                e."LAName",
                e."PhaseOfEducationName",
                e."AgeRangeLow",
                e."AgeRangeHigh",
                e."TotalPupils",
                e."SenTypes",
                e."TelephoneNum",
                e."Website",
                e."AddressStreet",
                e."AddressLocality",
                e."AddressAddress3",
                e."AddressTown",
                e."AddressPostcode",
                e."Easting",
                e."Northing",
                e."ISKS2",
                e."ISKS4",
                e."ISKS5"
            FROM public.v_establishment e
            WHERE e."URN" = @Urn;

            -- 2. KS4 establishment performance
            SELECT
                p."Id",
                p."Attainment8_Tot_Est_Current_Num_Coded",
                p."EngMaths59_Tot_Est_Current_Pct_Coded",
                p."More1FL_Tot_Est_Current_Pct_Coded"
            FROM public.v_establishment e
            LEFT JOIN public.v_establishment_performance p
                ON p."Id" = e."URN"
            WHERE e."URN" = @Urn;

            -- 3. KS4 local authority performance
            SELECT
                la."Id",
                la."Attainment8_Tot_LA_Current_Num_Coded",
                la."EngMaths59_Tot_LA_Current_Pct_Coded"
            FROM public.v_establishment e
            LEFT JOIN public.v_la_performance la
                ON la."Id" = e."LAId"
            WHERE e."URN" = @Urn;

            -- 4. KS4 England performance
            SELECT
                eng."Id",
                eng."Attainment8_Tot_Eng_Current_Num_Coded",
                eng."EngMaths59_Tot_Eng_Current_Pct_Coded"
            FROM public.v_england_performance eng
            WHERE eng."Id" = 'National';

            -- 5. Establishment destinations
            SELECT
                d."Id",
                d."AllDest_Tot_Est_Current_Pct_Coded"
            FROM public.v_establishment e
            LEFT JOIN public.v_establishment_destinations d
                ON d."Id" = e."URN"
            WHERE e."URN" = @Urn;

            -- 6. Local authority destinations
            SELECT
                la."Id",
                la."AllDest_Tot_LA_Current_Pct_Coded"
            FROM public.v_establishment e
            LEFT JOIN public.v_la_destinations la
                ON la."Id" = e."LAId"
            WHERE e."URN" = @Urn;

            -- 7. England destinations
            SELECT
                eng."Id",
                eng."AllDest_Tot_Eng_Current_Pct_Coded"
            FROM public.v_england_destinations eng
            WHERE eng."Id" = 'National';

            -- 8. KS2 establishment performance
            SELECT
                p."Id",
                p."PTRWM_EXP_Est_Current_Pct_Coded",
                p."PTRWM_HIGH_Est_Current_Pct_Coded"
            FROM public.v_establishment e
            LEFT JOIN public.v_establishment_ks2_attainment p
                ON p."Id" = e."URN"
            WHERE e."URN" = @Urn;

            -- 9. KS2 local authority performance
            SELECT
                la."Id",
                la."PTRWM_EXP_LA_Current_Pct_Coded",
                la."PTRWM_HIGH_LA_Current_Pct_Coded"
            FROM public.v_establishment e
            LEFT JOIN public.v_la_ks2_attainment la
                ON la."Id" = e."LAId"
            WHERE e."URN" = @Urn;

            -- 10. KS2 England performance
            SELECT
                eng."Id",
                eng."PTRWM_EXP_Eng_Current_Pct_Coded",
                eng."PTRWM_HIGH_Eng_Current_Pct_Coded"
            FROM public.v_england_ks2_attainment eng
            WHERE eng."Id" = 'National';
            """;

        await using var connection = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);

        var command = new DapperCommandBuilder()
            .WithCommandText(sql)
            .WithParameters(new { Urn = urn })
            .Build(ct);

        using var result = await connection.QueryMultipleAsync(command).ConfigureAwait(false); //only hit database once

        var establishment = await result.ReadSingleOrDefaultAsync<Establishment>();

        if (establishment is null)
            throw new NotFoundException($"Establishment not found with URN: {urn}");

        var ks4Performance = await result.ReadSingleOrDefaultAsync<EstablishmentPerformance>();

        var ks4LAPerformance = await result.ReadSingleOrDefaultAsync<LAPerformance>();

        var ks4EnglandPerformance = await result.ReadSingleOrDefaultAsync<EnglandPerformance>();

        var destinations = await result.ReadSingleOrDefaultAsync<KS4EstablishmentDestinations>();

        var laDestinations = await result.ReadSingleOrDefaultAsync<KS4LADestinations>();

        var englandDestinations = await result.ReadSingleOrDefaultAsync<KS4EnglandDestinations>();

        var ks2Performance = await result.ReadSingleOrDefaultAsync<KS2EstablishmentPerformance>();

        var ks2LAPerformance = await result.ReadSingleOrDefaultAsync<KS2LAPerformance>();

        var ks2EnglandPerformance = await result.ReadSingleOrDefaultAsync<KS2EnglandPerformance>();

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
            KS2EnglandPerformance = ks2EnglandPerformance
        };
    }
}