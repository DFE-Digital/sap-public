using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Infrastructure.PostgresSearch;

public class PostgresSchoolSearchIndexReader : ISchoolSearchIndexReader
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public PostgresSchoolSearchIndexReader(IEstablishmentRepository establishmentRepository)
    {
        _establishmentRepository = establishmentRepository;
    }

    public async Task<SchoolSearchResults> SearchAsync(SearchQuery query, int maxResults = 10)
    {
        var (results, totalCount) = await _establishmentRepository.SearchAsync(query, maxResults);

        return new SchoolSearchResults(
            Count: totalCount,
            Results: [.. results.Select(e => EstablishmentServiceModel.Map(e).ToSchoolSearchDocument())]
        );
    }
}