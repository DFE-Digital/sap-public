using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.SchoolSearch;

public interface ISchoolSearchIndexReader
{
    Task<IList<SchoolSearchResult>> SearchAsync(string query, int maxResults = 10);
}
