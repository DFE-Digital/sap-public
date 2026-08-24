namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// The school-profile phase a destination belongs to. Used for grouping/testing and to
/// help identify pages that must be hidden by the feature flags. Label
/// text itself is stored explicitly on each <see cref="PaginationDestination"/> because
/// different sections use different phase-prefix wording (e.g. "Secondary: Admissions"
/// vs "Secondary academic performance: Progress and attainment").
/// </summary>
public enum SchoolPhase
{
    None,
    Primary,
    Secondary,
    SixteenToNineteen
}

/// <summary>
/// A single, configured entry in the school-profile sitemap. The order of entries in
/// <see cref="SchoolProfileSitemap.Destinations"/> is the pagination order. New destinations are supported by
/// adding entries here, without changing pagination logic.
/// </summary>
/// <param name="Key">Stable identifier for the destination. Used to locate the current page and, for variable destinations, to look up availability in <see cref="PaginationContext.VariableDestinationAvailability"/>.</param>
/// <param name="Route">The route name used to generate the link (asp-route value).</param>
/// <param name="Phase">The phase this destination belongs to (for grouping/feature-flag checks).</param>
/// <param name="GetLabel">Produces the full, phase-aware pagination label, as it should be displayed (e.g. "Secondary: Admissions", "16 to 19 academic performance: Level 3 qualifications"). Takes the context so labels such as "About the school" / "About the school or college" can vary by establishment shape.</param>
/// <param name="IsAvailable">Predicate deciding whether this destination participates in pagination for a given context.</param>
/// <param name="IsVariable">When true, this destination is only available when explicitly marked available via <see cref="PaginationContext.VariableDestinationAvailability"/> (in addition to satisfying <see cref="IsAvailable"/>).</param>
public sealed record PaginationDestination(
    string Key,
    string Route,
    SchoolPhase Phase,
    Func<PaginationContext, string> GetLabel,
    Func<PaginationContext, bool> IsAvailable,
    bool IsVariable = false)
{
    public bool IsAvailableFor(PaginationContext context) =>
        IsAvailable(context) && (!IsVariable || context.IsVariableDestinationAvailable(Key));

    public string LabelFor(PaginationContext context) => GetLabel(context);
}
