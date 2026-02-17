using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader) : ISchoolSearchService
{
    private const int MaxResults = 1000;

    public async Task<SchoolSearchResults> SearchAsync(string query)
    {
        var searchResults = await indexReader.SearchAsync(query, MaxResults);

        var resultList = new List<SchoolSearchResult>();
        var results = new SchoolSearchResults(
            searchResults.Count,
            searchResults);

        return results;
    }
}
