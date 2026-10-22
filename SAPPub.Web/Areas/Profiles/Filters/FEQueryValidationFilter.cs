using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Areas.Profiles.Filters;

public class FEQueryValidationFilter(IEstablishmentService? establishmentService) : PhaseQueryValidationFilter(establishmentService)
{
    protected override bool IsInPhase(EstablishmentMinimumServiceModel establishment) => establishment.IsKS5;
}