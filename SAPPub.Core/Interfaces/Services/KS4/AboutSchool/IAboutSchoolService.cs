using SAPPub.Core.ServiceModels.KS4.AboutSchool;

namespace SAPPub.Core.Interfaces.Services.KS4.AboutSchool;

public interface IAboutSchoolService
{
    Task<AboutSchoolModel> GetAboutSchoolDetailsAsync(string urn, CancellationToken ct = default);

    Task<IEnumerable<AboutSchoolComparisonModel>> GetAboutSchoolForComparisonAsync(IEnumerable<string> urns, CancellationToken ct = default);
}
