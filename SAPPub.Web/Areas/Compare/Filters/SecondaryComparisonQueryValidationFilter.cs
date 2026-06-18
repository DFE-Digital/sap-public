using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Web.Areas.Compare.Filters;

public class SecondaryComparisonQueryValidationFilter(IEstablishmentService establishmentService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("urns", out var urnsObj) || urnsObj is not List<string> urnList || urnList.Count == 0)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var establishments = (await Task.WhenAll(
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
            })))
            .Where(est => est is not null)
            .Select(est => est!);

        var secondaryEstablishmentUrns = establishments
            .Where(est => est.IsKS4)
            .Select(est => est!.URN)
            .ToList();

        if (secondaryEstablishmentUrns is null || secondaryEstablishmentUrns.Count < 2 || secondaryEstablishmentUrns.Count > 6)
        {
            context.Result = new NotFoundResult();
            return;
        }
        context.ActionArguments["urns"] = secondaryEstablishmentUrns;

        await next();
    }
}
