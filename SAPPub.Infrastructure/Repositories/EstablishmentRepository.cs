using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly IGenericRepository<Establishment> _repo;
        private readonly ILogger<EstablishmentRepository> _logger;

        public EstablishmentRepository(
            IGenericRepository<Establishment> repo,
            ILogger<EstablishmentRepository> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Establishment> GetAllEstablishments()
        {
            // Keep only while we genuinely need to list; LIMIT 100 is already in DapperHelpers
            return _repo.ReadAll() ?? Enumerable.Empty<Establishment>();
        }

        public Establishment GetEstablishment(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new Establishment();

            return _repo.Read(urn) ?? new Establishment();
        }
    }
}
