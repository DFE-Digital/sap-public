using SAPPub.Core.Entities.KS4.Absence;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Absence
{
    public interface IEnglandAbsenceRepository
    {
        Task<EnglandAbsence> GetEnglandAbsenceAsync(CancellationToken ct = default);
    }
}