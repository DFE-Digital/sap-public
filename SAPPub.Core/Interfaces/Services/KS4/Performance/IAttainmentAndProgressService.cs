using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance;

public interface IAttainmentAndProgressService
{
    Task<AttainmentAndProgressModel> GetAttainmentAndProgressAsync(string urn, AcademicYearSelection selectedYear, CancellationToken ct = default);
}
