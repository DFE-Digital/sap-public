using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchIndexReader
{
    Task<SchoolSearchResults> SearchAsync(string query, int maxResults = 10);
}
