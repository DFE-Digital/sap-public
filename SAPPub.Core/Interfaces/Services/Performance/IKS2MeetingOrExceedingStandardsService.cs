using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS2MeetingOrExceedingStandardsService
{
    Task<KS2MeetingOrExceedingStandardsModel> GetMeetingOrExceedingStandardsPercentages(string urn, string LAId, CancellationToken ct = default);
}
