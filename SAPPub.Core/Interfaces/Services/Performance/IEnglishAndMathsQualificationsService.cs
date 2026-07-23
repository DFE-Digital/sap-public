using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IEnglishAndMathsQualificationsService
{
    Task<EnglishMathsQualificationModel> GetAdvancedLevelQualificationDetailsAsync(string urn, CancellationToken ct = default);
}
