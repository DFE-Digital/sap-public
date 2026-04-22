using SAPPub.Core.Helpers;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Models.Search;

public class SearchParamsModel
{
    private const string PostcodeSearchValidationRegex = """^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})$""";
    public string? NameSearchTerm { get; set; }

    [RegularExpression(PostcodeSearchValidationRegex, ErrorMessage = "Enter a full postcode")]
    public string? LocationSearchTerm { get; set; }
    public int Distance { get; set; } = 3;
}

public class SearchResultsViewModel
{
    public SearchParamsModel SearchParams { get; set; } = new SearchParamsModel();
    public int SearchResultsCount { get; set; }
    public string Heading => $"{SearchResultsCount} {(SearchResultsCount == 1 ? "result" : "results")} {HeadingClause2}";
    public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

    private string? HeadingClause2 =>
        !String.IsNullOrEmpty(SearchParams?.NameSearchTerm)
            ? $"for '{SearchParams.NameSearchTerm}'" + (!String.IsNullOrEmpty(SearchParams.LocationSearchTerm) ? $" within {SearchParams.Distance} {(SearchParams.Distance == 1 ? "mile" : "miles")} of {SearchParams.LocationSearchTerm}" : String.Empty)
            : !String.IsNullOrEmpty(SearchParams?.LocationSearchTerm)
                ? $"within {SearchParams.Distance} {(SearchParams.Distance == 1 ? "mile" : "miles")} of  {SearchParams.LocationSearchTerm}"
                : String.Empty;

    public static List<SearchResult> FromServiceModel(IEnumerable<SchoolSearchResultServiceModel> serviceModel)
    {
        return serviceModel.Select(r => SearchResult.FromServiceModel(r)).ToList();
    }
}

public class SearchResult
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string EstablishmentNameClean => TextHelpers.CleanForUrl(EstablishmentName);

    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? GenderName { get; set; }
    public required DisplayField<DateOnly> ClosedDate { get; set; }
    public int? StatusCode { get; set; }
    public bool IsSchoolClosed => StatusCode == 2;

    public static SearchResult FromServiceModel(SchoolSearchResultServiceModel serviceModel)
    {
        return new SearchResult
        {
            URN = serviceModel.URN?.ToString() ?? string.Empty,
            EstablishmentName = serviceModel.EstablishmentName ?? string.Empty,
            Address = serviceModel.Address ?? string.Empty,
            ReligiousCharacter = serviceModel.ReligiousCharacterName ?? string.Empty,
            GenderName = serviceModel.GenderName ?? string.Empty,
            StatusCode = serviceModel.StatusCode,
            ClosedDate = serviceModel.ClosedDate.ToDisplayField()
        };
    }
}
