using SAPPub.Core.ServiceModels.Search;
using SAPPub.Core.Services.Search;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchService
{
    Task<SchoolSearchResultsServiceModel> SearchAsync(SearchQuery query);
}

