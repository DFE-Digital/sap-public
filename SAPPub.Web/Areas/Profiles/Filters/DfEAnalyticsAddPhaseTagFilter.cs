using Dfe.Analytics.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Filters;

/// <summary>
/// Adds the Establishment IsKS2/ IsKS4 / IsKS5 info to the context for logging to BigQuery
/// using the DfE Analytics package
/// </summary>
public class DfEAnalyticsAddPhaseTagFilter(IEstablishmentService establishmentService) : IAsyncActionFilter, IOrderedFilter
{
    private const string KS2 = "KS2";
    private const string KS4 = "KS4";
    private const string KS5 = "KS5";

    // make this filter low order priority so that it runs after any ValidationFilters
    public int Order => 5;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        EstablishmentMinimumServiceModel? establishment = null;

        if (context.Controller is IEstablishment controller)
        {
            establishment = controller.Establishment;
        }
        else
        {
            if (!context.ActionArguments.TryGetValue(RouteConstants.URN, out var urnObj) || urnObj is not string urn)
            {
                await next();
                return;
            }

            establishment = await establishmentService.GetEstablishmentMinimumAsync(urn);
        }

        var webRequestEvent = context.HttpContext.GetWebRequestEvent();
        if (webRequestEvent is null || establishment is null)
        {
            await next();
            return;
        }

        if (establishment.IsKS2)
        {
            webRequestEvent.AddTag(KS2);
        }
        if (establishment.IsKS4)
        {
            webRequestEvent.AddTag(KS4);
        }
        if (establishment.IsKS5)
        {
            webRequestEvent.AddTag(KS5);
        }

        await next();
    }
}
