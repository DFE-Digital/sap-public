using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Services.KS4.Admissions;

public sealed class EstablishmentAdmissionsService(
    IEstablishmentService establishmentService,
    ILaUrlsRepository laUrlsRepository) : IAdmissionsService
{
    public async Task<AdmissionsServiceModel> GetAdmissionsDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        var laUrls = !string.IsNullOrWhiteSpace(establishment.GSSLACode) ? await laUrlsRepository.GetLaAsync(establishment.GSSLACode, ct) : null;
        laUrls ??= !string.IsNullOrWhiteSpace(establishment.DistrictAdministrativeId) ? await laUrlsRepository.GetLaAsync(establishment.DistrictAdministrativeId, ct) : null;

        return new AdmissionsServiceModel(
            SchoolName: establishment.EstablishmentName,
            SchoolWebsite: establishment.Website,
            LAName: laUrls?.Name,
            LASchoolAdmissionsUrl: laUrls?.LAMainUrl,
            EstablishmentStatus: establishment.StatusCode.ToStatus()
        );
    }
}
