using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.Search;

public interface ISchoolSearchService
{
    Task<SchoolSearchResults> SearchAsync(string query);
}

