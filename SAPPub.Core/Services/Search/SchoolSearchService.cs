using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using System.Text.RegularExpressions;
using SAPPub.Core.Extensions;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService : ISchoolSearchService
{
    private const string PostcodeValidationRegex = """^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})$""";
    private readonly ISchoolSearchIndexReader _schoolSearchIndexReader;
    private readonly IPostcodeLookupService _postcodeLookupService;

    public SchoolSearchService(
        ISchoolSearchIndexReader schoolSearchIndexReader,
        IPostcodeLookupService postcodeLookupService)
    {
        _schoolSearchIndexReader = schoolSearchIndexReader;
        _postcodeLookupService = postcodeLookupService;
    }

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SchoolSearchServiceQuery query)
    {
        var pageNumber = query.PageNumber ?? 1;

        // Validate postcode if present
        if (query.Location != null && !Regex.IsMatch(query.Location, PostcodeValidationRegex))
        {
            return new SchoolSearchResultsServiceModel
            {
                PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
                {
                    TotalRecords = 0,
                    Records = [],
                    PagerInfo = new Pager(0, pageNumber, Constants.PageSize)
                },
                Status = SchoolSearchStatus.InvalidPostcode
            };
        }

        double? latitude = null;
        double? longitude = null;

        if (query.Location != null)
        {
            var postcodeResponse = await _postcodeLookupService.GetLatitudeAndLongitudeAsync(query.Location);
            if (postcodeResponse?.Error != null)
            {
                return new SchoolSearchResultsServiceModel
                {
                    PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
                    {
                        TotalRecords = 0,
                        Records = [],
                        PagerInfo = new Pager(0, pageNumber, Constants.PageSize)
                    },
                    Status = postcodeResponse.Error == "Postcode not found"
                        ? SchoolSearchStatus.PostcodeNotFound
                        : SchoolSearchStatus.PostcodeServiceError
                };
            }
            latitude = postcodeResponse?.Result?.Latitude;
            longitude = postcodeResponse?.Result?.Longitude;
        }

        var searchQuery = new SearchQuery
        {
            Name = query.Name,
            Latitude = (float?)latitude,
            Longitude = (float?)longitude,
            Distance = query.Distance,
            Page = pageNumber,
            PageSize = Constants.PageSize
        };

        var results = await _schoolSearchIndexReader.SearchAsync(searchQuery, Constants.PageSize);

        return new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = results.Count,
                Records = results.Results.Select(e => e.ToSchoolSearchResult()).ToList(),
                PagerInfo = new Pager(results.Count, pageNumber, Constants.PageSize)
            }
        };
    }
}