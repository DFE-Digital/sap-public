using Lucene.Net.Index;
using Lucene.Net.Search;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.SchoolSearch;
using SAPPub.Core.Interfaces.Services.Search;

namespace SAPPub.Infrastructure.LuceneSearch;

public class LuceneSchoolSearchIndexReader(LuceneIndexContext context, LuceneTokeniser luceneTokeniser, LuceneHighlighter highlighter) : ISchoolSearchIndexReader
{
    public async Task<SchoolSearchResults> SearchAsync(string query, int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(query)) return new SchoolSearchResults(Count: 0, Results: new List<SchoolSearchDocument>());

        var tokens = luceneTokeniser.Tokenise(query).ToList();
        if (!tokens.Any()) return new SchoolSearchResults(Count: 0, Results: new List<SchoolSearchDocument>());

        await Task.Yield();

        context.SearcherManager.MaybeRefresh();

        var searcher = context.SearcherManager.Acquire();

        try
        {
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
            var exactName = new TermQuery(new Term(nameof(Establishment.EstablishmentName), query))
            {
                Boost = 10f
            };

            //Combine
            var finalQuery = new BooleanQuery
            {
                { must, Occur.MUST },
                { phrase, Occur.SHOULD },
                { exactName, Occur.SHOULD }
            };

            var take = maxResults;

            var sort = new Sort(new SortField("EstablishmentNameSort", SortFieldType.STRING, reverse: false), SortField.FIELD_SCORE);

            var topDocs = searcher.Search(finalQuery, take, sort);

            // CML check this is the total, even with pagination?
            var results = new SchoolSearchResults(Count: topDocs.TotalHits, Results: new List<SchoolSearchDocument>());

            foreach (var sd in topDocs.ScoreDocs)
            {
                var doc = searcher.Doc(sd.Doc);
                var urn = doc.Get(nameof(Establishment.URN));
                var establishmentName = doc.Get(nameof(Establishment.EstablishmentName));
                var religiousCharacterName = doc.Get(nameof(Establishment.ReligiousCharacterName));
                var genderName = doc.Get(nameof(Establishment.GenderName));
                var address = doc.Get(nameof(Establishment.Address));

                var highlightedText = highlighter.HighlightText(finalQuery, establishmentName, nameof(Establishment.EstablishmentName));

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