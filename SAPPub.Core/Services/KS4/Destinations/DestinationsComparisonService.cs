using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Services.KS4.Destinations;

public class DestinationsComparisonService(IKS4DestinationsRepository kS4Destinations) : IDestinationsComparisonService
{
    public async Task<DestinationsComparisonResultModel> GetDestinationsDetailsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        var establishmentDestinationsDetailsTask = kS4Destinations.GetEstablishmentsDestinationsAsync(urns, ct);
        var englandDestinationsDetailsTask = kS4Destinations.GetEnglandDestinationsAsync(ct);

        await Task.WhenAll(establishmentDestinationsDetailsTask, englandDestinationsDetailsTask);

        var establishmentDestinations = await establishmentDestinationsDetailsTask;
        var englandDestinations = await englandDestinationsDetailsTask;

        var schoolDetails = urns.Select(urn => new SchoolDestinationDetails
        {
            URN = urn,
            PercentInEducationEmploymentOrTraining = establishmentDestinations.FirstOrDefault(x => x.Id == urn)?.AllDest_Tot_Est_Current_Pct
        });
        return new DestinationsComparisonResultModel
        {
            SchoolDetails = schoolDetails,
            EnglandPercentage = englandDestinations.AllDest_Tot_Eng_Current_Pct
        };
    }
}
