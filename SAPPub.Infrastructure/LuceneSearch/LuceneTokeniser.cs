using Lucene.Net.Analysis.TokenAttributes;
using SAPPub.Core.Entities;

namespace SAPPub.Infrastructure.LuceneSearch;

public class LuceneTokeniser(LuceneIndexContext context)
{
    public IEnumerable<string> Tokenise(string finalText)
    {
        if (string.IsNullOrWhiteSpace(finalText)) return [];

        var tokens = new List<string>();

        using var tokenStream = context.Analyzer.GetTokenStream(nameof(Establishment.EstablishmentName), finalText);
        var termAttr = tokenStream.AddAttribute<ICharTermAttribute>();
        tokenStream.Reset();

        while (tokenStream.IncrementToken())
        {
            tokens.Add(termAttr.ToString());
        }

        return tokens;
    }
}