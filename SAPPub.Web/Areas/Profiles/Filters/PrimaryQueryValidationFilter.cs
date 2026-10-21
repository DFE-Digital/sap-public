using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Areas.Profiles.Filters;

public class PrimaryQueryValidationFilter(IEstablishmentService? establishmentService) : PhaseQueryValidationFilter(establishmentService)
{
    protected override bool IsInPhase(EstablishmentMinimumServiceModel establishment) => establishment.IsKS2;
}