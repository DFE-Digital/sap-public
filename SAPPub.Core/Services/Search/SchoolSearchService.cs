using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader, IPostcodeLookupService postcodeLookupService) : ISchoolSearchService
{
    private const int MaxResults = 1000;

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SAPPub.Core.ServiceModels.Search.SearchQuery query)
    {
        var postcodeResponse = query.Location != null ? await postcodeLookupService.GetLatitudeAndLongitudeAsync(query.Location) : null;
        if (postcodeResponse != null && postcodeResponse.Error != null)
        {
            return new SchoolSearchResultsServiceModel
            {
                Count = 0,
                SchoolSearchResults = new List<SchoolSearchResultServiceModel>(),
                Status = postcodeResponse.Error == "Invalid postcode" ? SchoolSearchStatus.InvalidPostcode : SchoolSearchStatus.UnknownError
            };
        }

        var postcodeResult = postcodeResponse?.Result;
        var searchQuery = string.IsNullOrEmpty(query.Location)
            ? new ServiceModels.PostcodeLookup.SearchQuery { Name = query.Name }
            : new ServiceModels.PostcodeLookup.SearchQuery { Latitude = postcodeResult?.Latitude, Longitude = postcodeResult?.Longitude, Distance = query.Distance, Name = query.Name };

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
