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