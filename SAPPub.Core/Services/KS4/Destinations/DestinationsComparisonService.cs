using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Services.KS4.Destinations;

public class DestinationsComparisonService(
    IEstablishmentDestinationsService establishmentDestinationsService,
    IEnglandDestinationsService englandDestinationsService) : IDestinationsComparisonService
{
    public async Task<DestinationsComparisonResultModel> GetDestinationsDetailsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        var establishmentDestinationsDetailsTask = establishmentDestinationsService.GetEstablishmentDestinationsAsync(urns, ct);
        var englandDestinationsDetailsTask = englandDestinationsService.GetEnglandDestinationsAsync(ct);

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
