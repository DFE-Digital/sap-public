using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Services.KS4.Admissions;

public class EstablishmentAdmissionsService(
    IEstablishmentService establishmentService,
    ILaUrlsRepository laUrlsRepository) : IAdmissionsService
{
    public async Task<AdmissionsServiceModel?> GetAdmissionsDetailsAsync(string urn)
    {
        var establishment = establishmentService.GetEstablishment(urn);

        var laGssCode = establishment.GSSLACode;
        if (laGssCode is null)
        {
            return null;
        }
        var laUrls = await laUrlsRepository.GetLaAsync(laGssCode);
        //if (laUrls is null)
        //{
        //    return null;
        //}
        /*else*/
        return new AdmissionsServiceModel(
           LAName: laUrls!.Name ?? "LA name was null",
           LASchoolAdmissionsUrl: laUrls!.LAMainUrl ?? "LA Url was null");
    }
}
