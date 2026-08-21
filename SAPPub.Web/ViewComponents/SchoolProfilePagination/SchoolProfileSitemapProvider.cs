namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// Default <see cref="ISitemapProvider"/> backed by the static school-profile sitemap.
/// </summary>
public sealed class SchoolProfileSitemapProvider : ISitemapProvider
{
    public IReadOnlyList<PaginationDestination> Destinations => SchoolProfileSitemap.Destinations;
}
