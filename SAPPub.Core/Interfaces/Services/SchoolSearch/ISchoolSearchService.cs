using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.SchoolSearch;

public interface ISchoolSearchService
{
    Task<IReadOnlyList<SchoolSearchResult>> SearchAsync(string query);
    Task<IReadOnlyList<SchoolSearchResult>> SuggestAsync(string queryPart);
}

