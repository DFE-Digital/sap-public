using Dfe.Analytics.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Areas.Profiles.Filters;

/// <summary>
/// Adds the Establishment IsKS2/ IsKS4 / IsKS5 info to the context for logging to BigQuery
/// using the DfE Analytics package
/// </summary>
public class DfEAnalyticsAddPhaseTagFilter(IEstablishmentService establishmentService) : IAsyncActionFilter, IOrderedFilter
{
    public int Order => 2;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        EstablishmentServiceModel? establishment = null;

        if (context.Controller is IEstablishment controller)
        {
            establishment = controller.Establishment;
        }
        else
        {
            if (!context.ActionArguments.TryGetValue("urn", out var urnObj) || urnObj is not string urn)
            {
                await next();
                return;
            }

            establishment = await establishmentService.GetEstablishmentAsync(urn);
        }

        var webRequestEvent = context.HttpContext.GetWebRequestEvent();
        if (webRequestEvent is null || establishment is null)
        {
            await next();
            return;
        }

        if (establishment.IsKS2)
        {
            webRequestEvent.AddTag("KS2");
        }
        if (establishment.IsKS4)
        {
            webRequestEvent.AddTag("KS4");
        }
        if (establishment.IsKS5)
        {
            webRequestEvent.AddTag("KS5");
        }

        await next();
    }
}
