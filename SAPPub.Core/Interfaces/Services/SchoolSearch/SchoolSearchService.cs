using SAPPub.Core.Entities.SchoolSearch;

namespace SAPPub.Core.Interfaces.Services.SchoolSearch;

public class SchoolSearchService(ISchoolSearchIndexReader indexReader, IEstablishmentService _establishmentService) : ISchoolSearchService
{
    private const int MaxResults = 1000;
    private const int MaxSuggestions = 10;

    public async Task<SchoolSearchResults> SearchAsync(string query)
    {
        var searchResults = await indexReader.SearchAsync(query, MaxResults);

        var resultList = new List<SchoolSearchResult>();
        var results = new SchoolSearchResults(
            searchResults.Count,
            searchResults.Select(sr =>
            {
                // CML TODO  - might want the indexed docs to contain all the fields required to avoid this extra call to the establishment service
                var school = _establishmentService.GetEstablishment(sr.urn.ToString());
                return SchoolSearchResult.FromNameAndEstablishment(sr.resultText, school);
            }).ToList()
        );

        return results;
    }

    public async Task<IReadOnlyList<SchoolSearchResult>> SuggestAsync(string queryPart)
    {
        var searchResults = await indexReader.SearchAsync(queryPart, MaxSuggestions);

        var results = new List<SchoolSearchResult>();

        if (searchResults.Count == 0) return results;

        // CML TODO will just need the name?
        foreach (var (urn, schoolName) in searchResults)
        {
            var school = _establishmentService.GetEstablishment(urn.ToString());
            results.Add(SchoolSearchResult.FromNameAndEstablishment(schoolName, school));
        }

        return results;
    }
}
