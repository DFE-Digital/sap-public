using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface ILevel3QualificationsService
{
    Task<Level3QualificationModel> GetLevel3QualificationDetailsAsync(
        string urn,
        Level3 level3Qualification, 
        CancellationToken ct = default);
}
