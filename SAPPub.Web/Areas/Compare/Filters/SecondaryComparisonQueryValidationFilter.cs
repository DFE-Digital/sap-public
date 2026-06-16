using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Web.Areas.Compare.Filters;

public class SecondaryComparisonQueryValidationFilter(IEstablishmentService establishmentService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var urns = context.ActionArguments["urns"];
        if (urns is null || urns is not List<string> urnsList || urnsList.Count == 0)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var urnList = urns as List<string>;
        var establishments = await Task.WhenAll(
            urnList!.Select(async urn =>
            {
                try
                {
                    return await establishmentService.GetEstablishmentAsync(urn);
                }
                catch (NotFoundException)
                {
                    return null;
                }
            }));

        var secondaryEstablishmentUrns = establishments.Where(est => est != null && est.IsKS4).Select(est => est!.URN).ToList();
        if (secondaryEstablishmentUrns is null || secondaryEstablishmentUrns.Count < 2 || secondaryEstablishmentUrns.Count > 6)
        {
            context.Result = new NotFoundResult();
            return;
        }
        context.ActionArguments["urns"] = secondaryEstablishmentUrns;

        await next();
    }
}
