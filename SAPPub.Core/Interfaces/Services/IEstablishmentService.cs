using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Interfaces.Services;

public interface IEstablishmentService
{
    Task<IEnumerable<EstablishmentServiceModel>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default);

    Task<EstablishmentServiceModel> GetEstablishmentAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<EstablishmentServiceModel>> GetEstablishmentsAsync(IEnumerable<string> urns, CancellationToken ct = default);

}
