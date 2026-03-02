using SAPPub.Core.ServiceModels.PostcodeSearch;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchIndexReader
{
    Task<SchoolSearchResults> SearchAsync(SearchQuery query, int maxResults = 10);
}
