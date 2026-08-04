using Dfe.Analytics.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SAPPub.Web.Areas.Profiles.Filters;

/// <summary>
/// Adds the Establishment IsKS2/ IsKS4 / IsKS5 info to the context for logging to BigQuery
/// using the DfE Analytics package
/// </summary>
public class DfEAnalyticsAddPhaseTagFilterAttribute : ActionFilterAttribute, IOrderedFilter
{
    public int Order => 2;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is IEstablishment controller)
        {
            var establishment = controller.Establishment;
            var webRequestEvent = context.HttpContext.GetWebRequestEvent();
            if (webRequestEvent is null || establishment is null) // CML might need to make this async so can get the establishment data for requests without IEstablishment implemented
            {
                return;
            }
            if (establishment.IsKS2)
            {
                webRequestEvent.AddTag("IsKS2");
            }
            if (establishment.IsKS4)
            {
                webRequestEvent.AddTag("IsKS4");
            }
            if (establishment.IsKS5)
            {
                webRequestEvent.AddTag("IsKS5");
            }
        }
    }
}
