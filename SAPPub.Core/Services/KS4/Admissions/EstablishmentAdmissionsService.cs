using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Services.KS4.Admissions;

public sealed class EstablishmentAdmissionsService(
    IEstablishmentService establishmentService,
    ILAService lAService) : IAdmissionsService
{
    public async Task<AdmissionsServiceModel> GetAdmissionsDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        var laUrls = await lAService.GetLaUrlsAsync(establishment, ct);

        return new AdmissionsServiceModel(
            SchoolName: establishment.EstablishmentName,
            SchoolWebsite: establishment.Website,
            LAName: laUrls?.Name,
            LASchoolAdmissionsUrl: laUrls?.LAMainUrl,
            EstablishmentStatus: establishment.StatusCode.ToStatus(),
            IsKS2: establishment.IsKS2,
            IsKS4: establishment.IsKS4,
            IsKS5: establishment.IsKS5
        );
    }
}
