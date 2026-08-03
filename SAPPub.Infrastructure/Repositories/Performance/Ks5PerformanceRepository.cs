using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.Performance;

namespace SAPPub.Infrastructure.Repositories.Performance;

public class Ks5PerformanceRepository(
    IGenericRepository<KS5EstablishmentPerformance> establishmentRepo,
    IGenericRepository<KS5England5Performance> englandRepo,    
    IGenericRepository<KS5LAPerformance> laRepo) : IKs5PerformanceRepository
{
    private readonly IGenericRepository<KS5EstablishmentPerformance> _establishmentRepo = establishmentRepo
            ?? throw new ArgumentNullException(nameof(establishmentRepo));
    private readonly IGenericRepository<KS5England5Performance> _englandRepo = englandRepo
            ?? throw new ArgumentNullException(nameof(englandRepo));    
    private readonly IGenericRepository<KS5LAPerformance> _laRepo = laRepo
            ?? throw new ArgumentNullException(nameof(laRepo));

    public async Task<KS5England5Performance> GetEnglandPerformanceAsync(CancellationToken ct = default)
    {       
        return await _englandRepo.ReadSingleAsync(new { }, ct) ?? new KS5England5Performance();
    }

    public async Task<KS5EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return new KS5EstablishmentPerformance();

        return await _establishmentRepo.ReadAsync(urn, ct) ?? new KS5EstablishmentPerformance();
    }

    public async Task<KS5LAPerformance> GetLaPerformanceAsync(string laCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(laCode))
            return new KS5LAPerformance();

        return await _laRepo.ReadAsync(laCode, ct) ?? new KS5LAPerformance();
    }
}
