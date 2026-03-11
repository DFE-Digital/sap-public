using SAPPub.Core.Entities.KS4.Absence;

namespace SAPPub.Core.Interfaces.Services.KS4.Absence
{
    public interface ILAAbsenceService
    {
        Task<IEnumerable<LAAbsence>> GetAllLAAbsenceAsync(CancellationToken ct = default);
        Task<LAAbsence> GetLAAbsenceAsync(string id, CancellationToken ct = default);
    }
}
