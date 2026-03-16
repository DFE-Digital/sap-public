using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Infrastructure.LuceneSearch;

[ExcludeFromCodeCoverage]
public static class LuceneExtensions
{
    public static void AddLuceneDependencies(this IServiceCollection services)
    {
        services.AddSingleton<LuceneIndexContext>();
        services.AddSingleton<LuceneSchoolSearchIndexWriter>();
        services.AddSingleton<LuceneSchoolSearchIndexReader>();
        services.AddSingleton<LuceneHighlighter>();
        services.AddSingleton<LuceneSynonymMapBuilder>();
        services.AddSingleton<LuceneTokeniser>();
        services.AddHostedService<StartupIndexBuilder>();
    }
}
