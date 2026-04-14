using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using System.Text.RegularExpressions;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader, IPostcodeLookupService postcodeLookupService) : ISchoolSearchService
{
    private const int MaxResults = 4000;
    private const string PostcodeValidationRegex = """^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})$""";

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SchoolSearchServiceQuery query)
    {
        if (query.Location != null && !Regex.IsMatch(query.Location, PostcodeValidationRegex))
        {
            return new SchoolSearchResultsServiceModel
            {
                Count = 0,
                SchoolSearchResults = new List<SchoolSearchResultServiceModel>(),
                Status = SchoolSearchStatus.InvalidPostcode
            };
        }
        var postcodeResponse = query.Location != null ? await postcodeLookupService.GetLatitudeAndLongitudeAsync(query.Location) : null;
        if (postcodeResponse != null && postcodeResponse.Error != null)
        {
            return new SchoolSearchResultsServiceModel
            {
                Count = 0,
                SchoolSearchResults = new List<SchoolSearchResultServiceModel>(),
                Status = postcodeResponse.Error == "Postcode not found" ? SchoolSearchStatus.PostcodeNotFound : SchoolSearchStatus.PostcodeServiceError
            };
        }

        var postcodeResult = postcodeResponse?.Result;
        var searchQuery = string.IsNullOrEmpty(query.Location)
            ? new ServiceModels.Search.InputModels.SearchQuery { Name = query.Name }
            : new ServiceModels.Search.InputModels.SearchQuery { Latitude = postcodeResult?.Latitude, Longitude = postcodeResult?.Longitude, Distance = query.Distance, Name = query.Name };

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
                ReligiousCharacterName = result.ReligiousCharacterName,
                Distance = postcodeResult is not null
                    ? MappingHelper.HaversineMiles(postcodeResult.Latitude, postcodeResult.Longitude, result.Latitude, result.Longitude)
                    : null,
                StatusCode = result.StatusCode,
                ClosedDate = result.ClosedDate
            }).Where(r => query.Location != null ? query.Distance.HasValue && r.Distance.HasValue && r.Distance.Value <= query.Distance : true).ToList()
        };

        return results;
    }
}
