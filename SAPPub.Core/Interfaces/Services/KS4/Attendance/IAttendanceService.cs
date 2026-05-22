using SAPPub.Core.ServiceModels.KS4.Attendance;

namespace SAPPub.Core.Interfaces.Services.KS4.Attendance;

public interface IAttendanceService
{
    Task<AttendanceModel> GetAttendenceDetailsAsync(string urn, CancellationToken ct = default);
}
