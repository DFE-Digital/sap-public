using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Filters;

public abstract class PhaseQueryValidationFilter(IEstablishmentService? establishmentService) : IAsyncActionFilter, IOrderedFilter
{
    // make this filter high order priority so that it runs first
    public int Order => 1;

    protected abstract bool IsInPhase(EstablishmentMinimumServiceModel establishment);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue(RouteConstants.URN, out var urnObj) || urnObj is not string urn)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (establishmentService is null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn);

        if (!IsInPhase(establishment))
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
