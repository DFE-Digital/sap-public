using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Web.Areas.Compare.Filters;

public class EnsureUrnsAreSecondaryAttribute : TypeFilterAttribute
{
    public EnsureUrnsAreSecondaryAttribute() : base(typeof(EnsureUrnsAreSecondaryFilter)) { }
}

public class EnsureUrnsAreSecondaryFilter(IEstablishmentService establishmentService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.TryGetValue("urns", out var urnsObj) && urnsObj is List<string> urns)
        {
            var establishments = await Task.WhenAll(urns.Select(urn => establishmentService.GetEstablishmentAsync(urn)));
            var secondaryEstablishmentUrns = establishments.Where(est => est != null && est.IsKeyStage4).Select(est => est.URN).ToList();
            if (secondaryEstablishmentUrns is null || secondaryEstablishmentUrns.Count < 2)
            {
                context.ActionArguments["urns"] = null;
                context.Result = new NotFoundObjectResult("Insufficient secondary establishments for comparison listed.");
                return;
            }
            context.ActionArguments["urns"] = secondaryEstablishmentUrns;
        }

        await next();
    }
}
