using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader) : ISchoolSearchService
{
    private const int MaxResults = 1000;

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SAPPub.Core.ServiceModels.Search.SearchQuery query)
    {
        // CML TODO: use postcode.io to convert postcode to lat/long
        float longitude = 54.9791529f;
        float latitude = -1.6106219f;
        var searchResults = await indexReader.SearchAsync(new Entities.SchoolSearch.SearchQuery(latitude, longitude, query.Name), MaxResults);

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
