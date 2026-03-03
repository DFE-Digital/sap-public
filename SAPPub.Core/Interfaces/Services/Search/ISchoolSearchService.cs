using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Core.Services.Search;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchService
{
    Task<SchoolSearchResultsServiceModel> SearchAsync(SchoolSearchServiceQuery query);
}

