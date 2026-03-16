using SAPPub.Core.ServiceModels.PostcodeSearch;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchIndexReader
{
    Task<SchoolSearchResults> SearchAsync(SearchQuery query, int maxResults = 10);
}
