using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.Specifications;
using SAPPub.Infrastructure.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using static SAPPub.Infrastructure.Repositories.EstablishmentRepository;

namespace SAPPub.Infrastructure.Repositories
{
    public class EstablishmentSimpleSearchRepository : IEstablishmentSearchRepository
    {
        private readonly IGenericRepository<Establishment> _establishemntDataSource;
        public EstablishmentSimpleSearchRepository(
            IGenericRepository<Establishment> establishmentDataSource)
        {
            _establishemntDataSource = establishmentDataSource ?? throw new ArgumentNullException(nameof(establishmentDataSource));
        }

        public async Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchAsync(SearchQuery query, int maxResults = 10, CancellationToken ct = default)
        {
            var allEstablishments = await _establishemntDataSource.ReadAllAsync(ct);

            var returnList = allEstablishments.Where(x => x.EstablishmentName.Contains(query.Name ?? "", StringComparison.OrdinalIgnoreCase)).ToList();

            return new(returnList, returnList.Count);
        }
    }
}
