using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchService
{
    Task<SchoolSearchResultsServiceModel> SearchAsync(SchoolSearchServiceQuery query);
}

