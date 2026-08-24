namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// Supplies the ordered set of pagination destinations for a given pagination
/// "flow" (e.g. school profile pages vs. comparison pages). Allows the same
/// resolver/ViewComponent code to be reused across different areas of the service.
/// </summary>
public interface ISitemapProvider
{
    /// <summary>
    /// The ordered list of destinations for this flow. Order determines pagination order.
    /// </summary>
    IReadOnlyList<PaginationDestination> Destinations { get; }
}
