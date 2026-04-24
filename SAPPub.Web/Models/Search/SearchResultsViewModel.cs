using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Constants;
using SAPPub.Web.ViewComponents.Pagination;

namespace SAPPub.Web.Models.Search;

public class SearchResultsViewModel
{
    public SearchParamsModel SearchParams { get; set; } = new SearchParamsModel();
    public int SearchResultsCount { get; set; }
    public string Heading => $"{SearchResultsCount} {(SearchResultsCount == 1 ? "result" : "results")} {HeadingClause2}";
    public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();
    public PaginationModel? Pagination { get; set; }

    private string? HeadingClause2 =>
        !string.IsNullOrEmpty(SearchParams?.NameSearchTerm)
            ? $"for '{SearchParams.NameSearchTerm}'" + (!string.IsNullOrEmpty(SearchParams.LocationSearchTerm) ? $" within {SearchParams.Distance} {(SearchParams.Distance == 1 ? "mile" : "miles")} of {SearchParams.LocationSearchTerm}" : string.Empty)
            : !string.IsNullOrEmpty(SearchParams?.LocationSearchTerm)
                ? $"within {SearchParams.Distance} {(SearchParams.Distance == 1 ? "mile" : "miles")} of  {SearchParams.LocationSearchTerm}"
                : string.Empty;

    public static SearchResultsViewModel FromServiceModel(SearchParamsModel searchModel, SchoolSearchResultsServiceModel? searchResultsServiceModel)
    {
        var searchResults = searchResultsServiceModel?.PagedResponse.Records.OrderBy(r => r.Distance);
        var pagerInfo = searchResultsServiceModel?.PagedResponse.PagerInfo;

        return new SearchResultsViewModel
        {
            SearchParams = searchModel,
            SearchResultsCount = pagerInfo?.TotalItems ?? 0,
            SearchResults = searchResults != null ? [.. searchResults.Select(SearchResult.FromServiceModel)] : [],
            Pagination = pagerInfo != null ? new PaginationModel 
            {
                PagerInfo = new Common.PagerViewModel
                {
                    TotalItems = pagerInfo.TotalItems,
                    CurrentPage = pagerInfo.CurrentPage,
                    PageSize = pagerInfo.PageSize,
                    TotalPages = pagerInfo.TotalPages,
                },
                RouteName = RouteConstants.SearchResults,
                RouteAttributes = new Dictionary<string, string?>
                {
                    { nameof(searchModel.NameSearchTerm), searchModel?.NameSearchTerm },
                    { nameof(searchModel.LocationSearchTerm), searchModel?.LocationSearchTerm }
                }
            } : null
        };
    }
}
