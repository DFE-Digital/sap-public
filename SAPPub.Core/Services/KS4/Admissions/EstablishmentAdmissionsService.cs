using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Services.KS4.Admissions;

public class EstablishmentAdmissionsService : IAdmissionsService
{
    public async Task<AdmissionsServiceModel> ExecuteAsync(string urn)
    {
        return new AdmissionsServiceModel(
            LAName: "Example Local Authority",
            LASchoolAdmissionsUrl: "https://www.example.com/school-admissions"
        );
    }
}
