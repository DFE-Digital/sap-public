using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.Performance;

namespace SAPPub.Infrastructure.Repositories.Performance;

public class Ks5PerformanceRepository(
    IGenericRepository<EstablishmentKs5Performance> establishmentRepo,
    IGenericRepository<EnglandKs5Performance> englandRepo,    
    IGenericRepository<LAKs5Performance> laRepo) : IKs5PerformanceRepository
{
    private readonly IGenericRepository<EstablishmentKs5Performance> _establishmentRepo = establishmentRepo
            ?? throw new ArgumentNullException(nameof(establishmentRepo));
    private readonly IGenericRepository<EnglandKs5Performance> _englandRepo = englandRepo
            ?? throw new ArgumentNullException(nameof(englandRepo));    
    private readonly IGenericRepository<LAKs5Performance> _laRepo = laRepo
            ?? throw new ArgumentNullException(nameof(laRepo));

    public async Task<EnglandKs5Performance> GetEnglandPerformanceAsync(CancellationToken ct = default)
    {       
        return await _englandRepo.ReadSingleAsync(new { }, ct) ?? new EnglandKs5Performance();
    }

    public async Task<EstablishmentKs5Performance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return new EstablishmentKs5Performance();

        return await _establishmentRepo.ReadAsync(urn, ct) ?? new EstablishmentKs5Performance();
    }

    public async Task<LAKs5Performance> GetLaPerformanceAsync(string laCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(laCode))
            return new LAKs5Performance();

        return await _laRepo.ReadAsync(laCode, ct) ?? new LAKs5Performance();
    }
}
