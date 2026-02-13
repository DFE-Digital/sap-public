using SAPPub.Core.Entities.KS4.Absence;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Absence
{
    public interface IEnglandAbsenceRepository
    {
        Task<EnglandAbsence> GetEnglandAbsenceAsync(CancellationToken ct = default);
    }
}