using SAPPub.Core.Entities.SchoolSearch;
using SAPPub.Core.Services.Search;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchIndexReader
{
    Task<SchoolSearchResults> SearchAsync(SearchQuery query, int maxResults = 10);
}
