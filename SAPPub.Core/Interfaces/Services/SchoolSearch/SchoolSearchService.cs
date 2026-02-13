using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.SchoolSearch;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader, IEstablishmentService _establishmentService) : ISchoolSearchService
{
    private const int MaxResults = 1000;
    private const int MaxSuggestions = 10;

    public async Task<IReadOnlyList<SchoolSearchResult>> SearchAsync(string query)
    {
        var searchResults = await indexReader.SearchAsync(query, MaxResults);

        var results = new List<SchoolSearchResult>();

        if (searchResults.Count == 0) return results;

        foreach (var (urn, schoolName) in searchResults)
        {
            var school = _establishmentService.GetEstablishment(urn.ToString());
            results.Add(SchoolSearchResult.FromNameAndEstablishment(schoolName, school));
        }

        return results;
    }

    public async Task<IReadOnlyList<SchoolSearchResult>> SuggestAsync(string queryPart)
    {
        var searchResults = await indexReader.SearchAsync(queryPart, MaxSuggestions);

        var results = new List<SchoolSearchResult>();

        if (searchResults.Count == 0) return results;

        foreach (var (urn, schoolName) in searchResults)
        {
            var school = _establishmentService.GetEstablishment(urn.ToString());
            results.Add(SchoolSearchResult.FromNameAndEstablishment(schoolName, school));
        }

        return results;
    }
}
