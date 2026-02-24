using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader) : ISchoolSearchService
{
    private const int MaxResults = 1000;

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SAPPub.Core.ServiceModels.Search.SearchQuery query)
    {
        // CML TODO: use postcode.io to convert postcode to lat/long
        var searchQuery = string.IsNullOrEmpty(query.Location)
            ? new Entities.SchoolSearch.SearchQuery(null, null, query.Name)
            : new Entities.SchoolSearch.SearchQuery(Latitude: -1.61392f, Longitude: 54.97753f, query.Name);

        var searchResults = await indexReader.SearchAsync(searchQuery, MaxResults);

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
