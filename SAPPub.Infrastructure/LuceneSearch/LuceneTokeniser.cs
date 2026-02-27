using Lucene.Net.Analysis.TokenAttributes;
using SAPPub.Core.Entities;

namespace SAPPub.Infrastructure.LuceneSearch;

public class LuceneTokeniser(LuceneIndexContext context)
{
    public IEnumerable<string> Tokenise(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return [];

        var tokens = new List<string>();

        using var tokenStream = context.Analyzer.GetTokenStream(nameof(Establishment.EstablishmentName), searchTerm);
        var termAttr = tokenStream.AddAttribute<ICharTermAttribute>();
        tokenStream.Reset();

        while (tokenStream.IncrementToken())
        {
            tokens.Add(termAttr.ToString());
        }

        return tokens;
    }
}