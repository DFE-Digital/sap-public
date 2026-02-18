using SAPPub.Core.Entities.KS4.Absence;

namespace SAPPub.Core.Interfaces.Services.KS4.Absence
{
    public interface IEnglandAbsenceService
    {
        Task<EnglandAbsence> GetEnglandAbsenceAsync(CancellationToken ct = default);
    }
}
