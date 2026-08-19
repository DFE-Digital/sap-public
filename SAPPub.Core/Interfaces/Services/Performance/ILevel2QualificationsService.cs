using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface ILevel2QualificationsService
{
    Task<Level2QualificationModel> GetLevel2QualificationDetailsAsync(
        string urn,
        Level2 level2Qualification,
        CancellationToken ct = default);
}
