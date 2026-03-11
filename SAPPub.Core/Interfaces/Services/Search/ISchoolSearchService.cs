using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchService
{
    Task<SchoolSearchResultsServiceModel> SearchAsync(string query);
}

