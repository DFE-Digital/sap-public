using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.KS4.Absence
{
    public sealed class EnglandAbsenceRepository : IEnglandAbsenceRepository
    {
        private readonly IGenericRepository<EnglandAbsence> _repo;
        private readonly ILogger<EnglandAbsenceRepository> _logger;

        public EnglandAbsenceRepository(
            IGenericRepository<EnglandAbsence> repo,
            ILogger<EnglandAbsenceRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EnglandAbsence> GetEnglandAbsenceAsync(CancellationToken ct = default)
        {
            return await _repo.ReadSingleAsync(new { }, ct) ?? new EnglandAbsence();
        }
    }
}
