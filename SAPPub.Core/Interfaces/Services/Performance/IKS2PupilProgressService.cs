using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS2PupilProgressService
{
    [Obsolete("GetPupilProgressAsync with this signature is deprecated, please use other GetPupilProgressAsync. Should not be used beyond Dec 10 2026")]
    Task<KS2PupilPerformance> GetPupilProgressAsync(string urn, AcademicYearSelection selectedYear, CancellationToken ct = default);

    Task<KS2PupilPerformanceBandings> GetPupilProgressAsync(string urn, CancellationToken ct = default);
}
