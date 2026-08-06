namespace SAPPub.Core.Specifications;

public sealed class SearchVisibilitySpecification : IEstablishmentSearchSpecification
{
    private readonly bool _includeKs5;
    private readonly bool _includeKS2;

    public SearchVisibilitySpecification(bool includeKs5, bool includeKS2)
    {
        _includeKs5 = includeKs5;
        _includeKS2 = includeKS2;
    }

    public string ToSqlPredicate()
    {
        var predicates = new List<string>();

        // Exclude KS5-only establishments unless feature is enabled
        // Always include if also KS4
        if (!_includeKs5)
        {
            predicates.Add(@"(""ISKS5"" IS NOT TRUE OR ""ISKS4"" IS TRUE)");
        }

        // Exclude KS2-only establishments unless feature is enabled
        // Always include if also KS4
        if (!_includeKS2)
        {
            predicates.Add(@"(""ISKS2"" IS NOT TRUE OR ""ISKS4"" IS TRUE)");
        }

        return predicates.Count > 0 
            ? string.Join(" AND ", predicates) 
            : "1=1";
    }
}
