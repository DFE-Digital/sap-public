using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Services.KS4.Admissions;

public class EstablishmentAdmissionsService(
    IEstablishmentRepository establishmentRepository,
    ILaUrlsRepository laUrlsRepository) : IAdmissionsService
{
    public async Task<AdmissionsServiceModel?> GetAdmissionsDetailsAsync(string urn)
    {
        var establishment = establishmentRepository.GetEstablishment(urn);
        if (establishment is null)
        {
            return null;
        }
        var laGssCode = establishment.LaGssCode;
        if (laGssCode is null)
        {
            return null;
        }
        var laUrls = await laUrlsRepository.GetLaAsync(laGssCode);
        if (laUrls is null)
        {
            return null;
        }
        else return new AdmissionsServiceModel(
            LAName: laUrls?.LaName,
            LASchoolAdmissionsUrl: laUrls?.GeneralUrl
        );
    }
}
