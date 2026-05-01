using SAPPub.Core.Entities;
using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace SAPPub.Core.Services.Search;

public class SchoolSearchService : ISchoolSearchService
{
    private const string PostcodeValidationRegex = """^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})$""";
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly IPostcodeLookupService _postcodeLookupService;

    public SchoolSearchService(
        IEstablishmentRepository establishmentRepository,
        IPostcodeLookupService postcodeLookupService)
    {
        _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
        _postcodeLookupService = postcodeLookupService ?? throw new ArgumentNullException(nameof(postcodeLookupService));
    }

    public async Task<SchoolSearchResultsServiceModel> SearchAsync(SchoolSearchServiceQuery query)
    {
        var pageNumber = query.PageNumber ?? 1;

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

        var postcodeResponse = query.Location != null
            ? await _postcodeLookupService.GetLatitudeAndLongitudeAsync(query.Location)
            : null;

        if (postcodeResponse != null && postcodeResponse.Error != null)
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

        var postcodeResult = postcodeResponse?.Result;
        List<SchoolSearchResultServiceModel> filteredSearchResults;
        int totalRecords = 0;


        // 1. Name only
        if (!string.IsNullOrWhiteSpace(query.Name) && string.IsNullOrWhiteSpace(query.Location))
        {
            (IEnumerable<Establishment> establishments, int count) = await _establishmentRepository.SearchByNameAsync(query.Name, pageNumber, Constants.PageSize);
            totalRecords = count;
            filteredSearchResults = establishments.Select(e => new SchoolSearchResultServiceModel

            {
                URN = e.URN,
                EstablishmentName = e.EstablishmentName,
                Address = e.Address,
                GenderName = e.GenderName,
                ReligiousCharacterName = e.ReligiousCharacterName,
                Distance = null,
                StatusCode = e.StatusCode,
                ClosedDate = e.ClosedDate.ToDateOnly()
            }).ToList();
        }
        // 2. Location only
        else if (string.IsNullOrWhiteSpace(query.Name) && postcodeResult is not null && query.Distance.HasValue)
        {
            (IEnumerable<Establishment> establishments, int count) = await _establishmentRepository.SearchByLocationAsync(
                 postcodeResult.Latitude,
                 postcodeResult.Longitude,
                 query.Distance.Value,
                 pageNumber,
                 Constants.PageSize);

            totalRecords = count;
            filteredSearchResults = establishments.Select(e => new SchoolSearchResultServiceModel
            {
                URN = e.URN,
                EstablishmentName = e.EstablishmentName,
                Address = e.Address,
                GenderName = e.GenderName,
                ReligiousCharacterName = e.ReligiousCharacterName,
                StatusCode = e.StatusCode,
                ClosedDate = e.ClosedDate.ToDateOnly()
            }).ToList();
        }
        // 3. Name + Location
        else if (!string.IsNullOrWhiteSpace(query.Name) && postcodeResult is not null && query.Distance.HasValue)
        {
            (IEnumerable<Establishment> establishments, int count) = await _establishmentRepository.SearchByNameAndLocationAsync(
               query.Name,
               postcodeResult.Latitude,
               postcodeResult.Longitude,
               query.Distance.Value,
               pageNumber,
               Constants.PageSize);

            totalRecords = count;
            filteredSearchResults = establishments.Select(e => new SchoolSearchResultServiceModel
            {
                URN = e.URN,
                EstablishmentName = e.EstablishmentName,
                Address = e.Address,
                GenderName = e.GenderName,
                ReligiousCharacterName = e.ReligiousCharacterName,
                StatusCode = e.StatusCode,
                ClosedDate = e.ClosedDate.ToDateOnly()
            }).ToList();
        }
        else
        {
            // No valid search parameters
            filteredSearchResults = new List<SchoolSearchResultServiceModel>();
        }

        var pager = new Pager(totalRecords, pageNumber, Constants.PageSize);

        var results = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = totalRecords,
                Records = filteredSearchResults.ToList(),
                PagerInfo = pager
            }
        };

        return results;
    }
}
