using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.Performance;

namespace SAPPub.Infrastructure.Repositories.Performance;

public class KS2PerformanceRepository(
    IGenericRepository<KS2EstablishmentPerformance> establishmentRepo,
    IGenericRepository<KS2EnglandPerformance> englandRepo,    
    IGenericRepository<KS2LAPerformance> laRepo) : IKS2PerformanceRepository
{
    private readonly IGenericRepository<KS2EstablishmentPerformance> _establishmentRepo = establishmentRepo
            ?? throw new ArgumentNullException(nameof(establishmentRepo));
    private readonly IGenericRepository<KS2EnglandPerformance> _englandRepo = englandRepo
            ?? throw new ArgumentNullException(nameof(englandRepo));    
    private readonly IGenericRepository<KS2LAPerformance> _laRepo = laRepo
            ?? throw new ArgumentNullException(nameof(laRepo));

    public async Task<KS2EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default)
    {       
        return await _englandRepo.ReadSingleAsync(new { }, ct) ?? new KS2EnglandPerformance();
    }

    public async Task<KS2EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return new KS2EstablishmentPerformance();

        return await _establishmentRepo.ReadAsync(urn, ct) ?? new KS2EstablishmentPerformance();
    }

    public async Task<KS2LAPerformance> GetLaPerformanceAsync(string laCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(laCode))
            return new KS2LAPerformance();

        return await _laRepo.ReadAsync(laCode, ct) ?? new KS2LAPerformance();
    }
}
