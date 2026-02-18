using SAPPub.Core.Entities.KS4.Absence;

namespace SAPPub.Core.Interfaces.Services.KS4.Absence
{
    public interface IEstablishmentAbsenceService
    {
        Task<IEnumerable<EstablishmentAbsence>> GetAllEstablishmentAbsenceAsync(CancellationToken ct = default);
        Task<EstablishmentAbsence> GetEstablishmentAbsenceAsync(string urn, CancellationToken ct = default);
    }
}
