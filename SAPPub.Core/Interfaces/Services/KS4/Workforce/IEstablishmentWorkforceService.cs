using SAPPub.Core.Entities.KS4.Workforce;

namespace SAPPub.Core.Interfaces.Services.KS4.Workforce
{
    public interface IEstablishmentWorkforceService
    {
        Task<IEnumerable<EstablishmentWorkforce>> GetAllEstablishmentWorkforceAsync(CancellationToken ct = default);
        Task<EstablishmentWorkforce> GetEstablishmentWorkforceAsync(string urn, CancellationToken ct = default);
    }
}
