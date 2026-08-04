using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Web.Areas.Profiles.Filters;

public class PrimaryQueryValidationFilter(IEstablishmentService establishmentService) : IAsyncActionFilter, IOrderedFilter
{
    public int Order => 1;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("urn", out var urnObj) || urnObj is not string urn)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var establishment = await establishmentService.GetEstablishmentAsync(urn);

        if (!establishment.IsKS2)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (context.Controller is IEstablishment controller)
        {
            controller.Establishment = establishment;
        }

        await next();
    }
}
