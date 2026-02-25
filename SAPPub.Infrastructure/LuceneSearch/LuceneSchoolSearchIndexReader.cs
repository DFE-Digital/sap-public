using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Spatial.Queries;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.PostcodeLookup;

namespace SAPPub.Infrastructure.LuceneSearch;

public class LuceneSchoolSearchIndexReader(LuceneIndexContext context, LuceneTokeniser luceneTokeniser) : ISchoolSearchIndexReader
{
    private List<BooleanClause> BuildNameQuery(string nameQueryString)
    {
        var tokens = luceneTokeniser.Tokenise(nameQueryString).ToList();
        if (!tokens.Any()) return new List<BooleanClause>();

        // Strict query for all but the LAST token
        var must = new BooleanQuery();
        foreach (var t in tokens.Take(tokens.Count - 1))
        {
            must.Add(new TermQuery(new Term(nameof(Establishment.EstablishmentName), t)), Occur.MUST);
        }

        // Add the LAST token as a PrefixQuery for partial matching
        must.Add(new PrefixQuery(new Term(nameof(Establishment.EstablishmentName), tokens.Last())), Occur.MUST);

        //Phrase boost – original order
        var phrase = new PhraseQuery { Slop = 2, Boost = 5f };
        foreach (var t in tokens)
        {
            phrase.Add(new Term(nameof(Establishment.EstablishmentName), t));
        }

        // Exact name
        var exactName = new TermQuery(new Term(nameof(Establishment.EstablishmentName), nameQueryString))
        {
            Boost = 10f
        };

        return new List<BooleanClause> {
            new (must, Occur.MUST),
            new (phrase, Occur.SHOULD),
            new (exactName, Occur.SHOULD)
        };
    }

    private List<BooleanClause> BuildDistanceQuery(float latitude, float longitude, int radiusMiles)
    {
        // Convert miles to degrees for Spatial4n circle
        double radiusDegrees = MappingHelper.MilesToDegrees(radiusMiles);

        // Build the circle and query
        var center = context.SpatialContext.MakePoint(longitude, latitude);
        var circle = context.SpatialContext.MakeCircle(center.X, center.Y, radiusDegrees);

        // Intersects ~= within the circle for points
        var args = new SpatialArgs(SpatialOperation.Intersects, circle);
        Query distanceQuery = context.GeoStrategy.MakeQuery(args);

        // Execute (without name filtering)
        return new List<BooleanClause> { new BooleanClause(distanceQuery, Occur.MUST) };
    }

    public async Task<SchoolSearchResults> SearchAsync(SearchQuery searchQuery, int maxResults = 10)
    {
        await Task.Yield();

        if (string.IsNullOrEmpty(searchQuery.Name) && (!searchQuery.Latitude.HasValue || !searchQuery.Longitude.HasValue))
        {
            return new SchoolSearchResults(Count: 0, Results: new List<SchoolSearchDocument>());
        }

        context.SearcherManager.MaybeRefresh();
        var searcher = context.SearcherManager.Acquire();
        var take = maxResults;
        List<BooleanClause> queryTerms = [];
        Sort? sort = null;
        TopDocs? documentResults;
        try
        {
            if (!string.IsNullOrEmpty(searchQuery.Name))
            {
                queryTerms = BuildNameQuery(searchQuery.Name);

                sort = new Sort(new SortField("EstablishmentNameSort", SortFieldType.STRING, reverse: false), SortField.FIELD_SCORE);
            }
            if (searchQuery.Latitude.HasValue && searchQuery.Longitude.HasValue)
            {
                var distanceQuery = BuildDistanceQuery(searchQuery.Latitude.Value, searchQuery.Longitude.Value, searchQuery.Distance ?? 3);
                distanceQuery.ForEach(_ => queryTerms.Add(_));
            }

            // Construct BooleanQuery from these clauses
            var finalQuery = new BooleanQuery();
            queryTerms.ForEach(_ => finalQuery.Add(_));

            documentResults = sort is null ? searcher.Search(finalQuery, take) : searcher.Search(finalQuery, take, sort);
            var results = new SchoolSearchResults(Count: documentResults.TotalHits, Results: new List<SchoolSearchDocument>());

            foreach (var sd in documentResults.ScoreDocs)
            {
                var doc = searcher.Doc(sd.Doc);
                var urn = doc.Get(nameof(Establishment.URN));
                var establishmentName = doc.Get(nameof(Establishment.EstablishmentName));
                var religiousCharacterName = doc.Get(nameof(Establishment.ReligiousCharacterName));
                var genderName = doc.Get(nameof(Establishment.GenderName));
                var address = doc.Get(nameof(Establishment.Address));

                results.Results.Add(new SchoolSearchDocument(urn, establishmentName, address, genderName, religiousCharacterName));
            }

            return results;
        }
        finally
        {
            context.SearcherManager.Release(searcher);
        }
    }
}