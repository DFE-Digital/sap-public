using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using SAPPub.Web.Constants;

namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

public class SchoolProfilePagination : ViewComponent
{
    private readonly ISchoolProfilePaginationResolver _resolver;
    private readonly IFeatureManager _featureManager;

    public SchoolProfilePagination(ISchoolProfilePaginationResolver resolver, IFeatureManager featureManager)
    {
        _resolver = resolver;
        _featureManager = featureManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(SchoolProfilePaginationModel model)
    {
        SchoolProfilePaginationResult result;
        IDictionary<string, string> routeAttributes;

        if (model is null)
        {
            result = SchoolProfilePaginationResult.Empty;
            routeAttributes = new Dictionary<string, string>();
        }
        else
        {
            // Feature-flag state is resolved centrally
            var isPrimaryEnabled = await _featureManager.IsEnabledAsync(Constants.Constants.EnablePrimary);
            var is16To19Enabled = await _featureManager.IsEnabledAsync(Constants.Constants.Enable16to19);
            var isOverviewEnabled = await _featureManager.IsEnabledAsync(Constants.Constants.EnableOverview);

            var context = new PaginationContext
            {
                IsKS2 = model.IsKS2,
                IsKS4 = model.IsKS4,
                IsKS5 = model.IsKS5,
                IsPrimaryEnabled = isPrimaryEnabled,
                Is16To19Enabled = is16To19Enabled,
                IsOverviewEnabled = isOverviewEnabled,
                VariableDestinationAvailability = model.VariableDestinationAvailability ?? new Dictionary<string, bool>()
            };

            result = _resolver.Resolve(model.CurrentRoute, context);
            routeAttributes = model.RouteAttributes;
        }

        var viewModel = new SchoolProfilePaginationViewModel
        {
            Result = result,
            RouteAttributes = routeAttributes
        };

        return View("~/ViewComponents/SchoolProfilePagination/Default.cshtml", viewModel);
    }
}
