using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IAdvancedLevelQualificationsService
{
    Task<AdvancedLevelQualificationModel> GetAdvancedLevelQualificationDetailsAsync(
        string urn,
        Level3 level3Qualification, 
        CancellationToken ct = default);
}
