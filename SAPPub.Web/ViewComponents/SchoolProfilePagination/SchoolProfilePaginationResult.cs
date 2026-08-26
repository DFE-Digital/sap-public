namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// A single Previous/Next link target within the school-profile bottom pagination.
/// </summary>
/// <param name="Route">The route name to link to (used with asp-route).</param>
/// <param name="Label">The phase-aware label to display, e.g. "Secondary: Admissions".</param>
public sealed record PaginationLink(string Route, string Label);

/// <summary>
/// The resolved Previous/Next links for the current destination. Either link may be
/// null - the pagination component will render only the links that exist
/// (no broken/empty link, only valid links shown at either end
/// of the sequence).
/// </summary>
public sealed record SchoolProfilePaginationResult(PaginationLink? Previous, PaginationLink? Next)
{
    public static readonly SchoolProfilePaginationResult Empty = new(null, null);
}
