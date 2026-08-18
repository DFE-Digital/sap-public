namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

public interface ISchoolProfilePaginationResolver
{
    /// <summary>
    /// Resolves the Previous/Next destinations adjacent to <paramref name="currentRoute"/>
    /// within the school-profile sitemap, filtered to only the destinations available
    /// for <paramref name="context"/>. Returns <see cref="SchoolProfilePaginationResult.Empty"/>
    /// if the current route is not a recognised, available destination.
    /// </summary>
    SchoolProfilePaginationResult Resolve(string currentRoute, PaginationContext context);
}

/// <summary>
/// Config-driven resolver: filters <see cref="SchoolProfileSitemap.Destinations"/>
/// to those available for the given context, then returns the entries immediately
/// before/after the current one. Adding new destinations only requires changes to the sitemap configuration.
/// </summary>
public sealed class SchoolProfilePaginationResolver : ISchoolProfilePaginationResolver
{
    public SchoolProfilePaginationResult Resolve(string currentRoute, PaginationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (string.IsNullOrEmpty(currentRoute))
        {
            return SchoolProfilePaginationResult.Empty;
        }

        var available = SchoolProfileSitemap.Destinations
            .Where(d => d.IsAvailableFor(context))
            .ToList();

        var currentIndex = available.FindIndex(d => d.Route == currentRoute);
        if (currentIndex < 0)
        {
            return SchoolProfilePaginationResult.Empty;
        }

        var previous = currentIndex > 0 ? ToLink(available[currentIndex - 1], context) : null;
        var next = currentIndex < available.Count - 1 ? ToLink(available[currentIndex + 1], context) : null;

        return new SchoolProfilePaginationResult(previous, next);
    }

    private static PaginationLink ToLink(PaginationDestination destination, PaginationContext context) =>
        new(destination.Route, destination.LabelFor(context));
}
