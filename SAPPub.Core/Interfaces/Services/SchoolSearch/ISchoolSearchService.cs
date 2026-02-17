using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.SchoolSearch;

public interface ISchoolSearchService
{
    Task<SchoolSearchResults> SearchAsync(string query);
}

