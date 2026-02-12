using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Interfaces.Services.KS4.Admissions;

public interface IAdmissionsService
{
    public Task<AdmissionsServiceModel?> GetAdmissionsDetailsAsync(string urn);
}
