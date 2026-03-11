using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader) : ISchoolSearchService
{
    private const int MaxResults = 1000;

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(string query)
    {
        var searchResults = await indexReader.SearchAsync(query, MaxResults);

        var resultList = new List<SchoolSearchResultServiceModel>();
        var results = new SchoolSearchResultsServiceModel
        {
            Count = searchResults.Count,
            SchoolSearchResults = searchResults.Results.Select(result => new SchoolSearchResultServiceModel
            {
                URN = result.URN,
                EstablishmentName = result.EstablishmentName,
                Address = result.Address,
                GenderName = result.GenderName,
                ReligiousCharacterName = result.ReligiousCharacterName
            }).ToList()
        };

        return results;
    }
}
