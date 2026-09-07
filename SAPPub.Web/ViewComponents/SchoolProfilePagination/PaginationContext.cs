namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// Describes what school-profile content is available for the current establishment,
/// used to filter the <see cref="SchoolProfileSitemap"/> down to the destinations that
/// should participate in pagination for this request.
/// </summary>
public class PaginationContext
{
    public required bool IsKS2 { get; set; }

    public required bool IsKS4 { get; set; }

    public required bool IsKS5 { get; set; }

    /// <summary>
    /// Reflects the "EnablePrimary" feature flag. Primary destinations are excluded
    /// from pagination entirely while this is false, regardless of IsKS2.
    /// </summary>
    public bool IsPrimaryEnabled { get; set; }

    /// <summary>
    /// Reflects the "Enable16to19" feature flag. 16-19 destinations are excluded
    /// from pagination entirely while this is false, regardless of IsKS5.
    /// </summary>
    public bool Is16To19Enabled { get; set; }

    /// <summary>
    /// Reflects the "Overview" feature flag. 
    /// </summary>
    public bool IsOverviewEnabled { get; set; }

    /// <summary>
    /// Variable sub-tab/page availability flags, keyed by the destination Key in
    /// <see cref="SchoolProfileSitemap"/>. A destination with a variable flag is only
    /// included when its key is present here with a true value. Destinations that are
    /// not variable (i.e. always available when their phase/section is available)
    /// should not be added to this collection.
    /// </summary>
    public IReadOnlyDictionary<string, bool> VariableDestinationAvailability { get; set; }
        = new Dictionary<string, bool>();

    public bool IsVariableDestinationAvailable(string key) =>
        !VariableDestinationAvailability.TryGetValue(key, out var isAvailable) || isAvailable;
}
