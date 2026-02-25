using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader, IPostcodeLookupService postcodeLookupService) : ISchoolSearchService
{
    private const int MaxResults = 1000;

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SAPPub.Core.ServiceModels.Search.SearchQuery query)
    {
        var postcodeResponse = await postcodeLookupService.GetLatitudeAndLongitudeAsync(query.Location);
        var postcodeResult = postcodeResponse?.result; // CML TODO handle errors in response
        var searchQuery = string.IsNullOrEmpty(query.Location)
            ? new Entities.SchoolSearch.SearchQuery(null, null, query.Name)
            : new Entities.SchoolSearch.SearchQuery(Latitude: postcodeResult?.latitude, Longitude: postcodeResult?.longitude, query.Name);

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
