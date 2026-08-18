using SAPPub.Web.Areas.Profiles.ViewModels;
using SAPPub.Web.Models;

namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// Input model for the <see cref="SchoolProfilePagination"/> ViewComponent.
/// </summary>
public class SchoolProfilePaginationModel
{
    /// <summary>
    /// The route name of the page/tab currently being displayed. Must match a
    /// destination's Route in <see cref="SchoolProfileSitemap.Destinations"/>.
    /// </summary>
    public required string CurrentRoute { get; set; }

    public required bool IsKS2 { get; set; }

    public required bool IsKS4 { get; set; }

    public required bool IsKS5 { get; set; }

    public required IDictionary<string, string> RouteAttributes { get; set; }

    /// <summary>
    /// Variable sub-tab/page availability flags, keyed by destination Key in
    /// <see cref="SchoolProfileSitemap"/>. See <see cref="PaginationContext.VariableDestinationAvailability"/>.
    /// </summary>
    public IReadOnlyDictionary<string, bool>? VariableDestinationAvailability { get; set; }

    /// <summary>
    /// Builds a model from the common profile view model flags, so consuming views
    /// only need to supply their current route and (optionally) variable destination
    /// flags. Feature-flag state is resolved centrally by the
    /// <see cref="SchoolProfilePagination"/> ViewComponent itself.
    /// </summary>
    public static SchoolProfilePaginationModel From(
        ProfileBaseViewModel viewModel,
        string currentRoute,
        IReadOnlyDictionary<string, bool>? variableDestinationAvailability = null) =>
        From(viewModel.IsKS2, viewModel.IsKS4, viewModel.IsKS5, viewModel.RouteAttributes, currentRoute, variableDestinationAvailability);

    /// <summary>
    /// Overload for views whose model derives from <see cref="BaseViewModel"/>
    /// rather than <see cref="ProfileBaseViewModel"/>.
    /// </summary>
    public static SchoolProfilePaginationModel From(
        BaseViewModel viewModel,
        string currentRoute,
        IReadOnlyDictionary<string, bool>? variableDestinationAvailability = null) =>
        From(viewModel.IsKS2, viewModel.IsKS4, viewModel.IsKS5, viewModel.RouteAttributes, currentRoute, variableDestinationAvailability);

    private static SchoolProfilePaginationModel From(
        bool isKS2,
        bool isKS4,
        bool isKS5,
        IDictionary<string, string> routeAttributes,
        string currentRoute,
        IReadOnlyDictionary<string, bool>? variableDestinationAvailability)
    {
        return new SchoolProfilePaginationModel
        {
            CurrentRoute = currentRoute,
            RouteAttributes = routeAttributes,
            IsKS2 = isKS2,
            IsKS4 = isKS4,
            IsKS5 = isKS5,
            VariableDestinationAvailability = variableDestinationAvailability
        };
    }
}
