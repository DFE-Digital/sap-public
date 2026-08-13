using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS2PupilProgressService
{
    Task<AttainmentAndProgressModel> GetPupilProgressAsync(string urn, AcademicYearSelection selectedYear, CancellationToken ct = default);
}
