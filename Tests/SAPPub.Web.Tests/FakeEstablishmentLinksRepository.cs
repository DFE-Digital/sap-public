using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;

namespace SAPPub.Web.Tests;

public class FakeEstablishmentLinksRepository : IEstablishmentLinksRepository
{
    private static readonly List<(string, EstablishmentLinks)> EstablishmentLinks = new()
    {
        ("145744", new EstablishmentLinks
        {
            Urn = "145744",
            LinkName = "Predecessor 1 to Abbey Park School",
            LinkUrn = "178965",
            LinkType = "Predecessor"
        }),
        ("145744", new EstablishmentLinks
        {
            Urn = "145744",
            LinkName = "Predecessor 2 to Abbey Park School",
            LinkUrn = "178966",
            LinkType = "Predecessor"
        })
    };

    Task<IEnumerable<EstablishmentLinks>?> IEstablishmentLinksRepository.GetLinksAsync(string urn, CancellationToken ct)
    {
        var links = EstablishmentLinks.Where(x => x.Item1 == urn).Select(x => x.Item2);
        return Task.FromResult<IEnumerable<EstablishmentLinks>?>(links);
    }
}
