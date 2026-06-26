using SAPPub.Core.Interfaces.Services.Compare;
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

        return new DestinationsComparisonResultModel
        {
            SchoolDetails = new List<SchoolDestinationDetails>(
                establishmentDestinations.Select(ed => new SchoolDestinationDetails
                {
                    URN = ed.Id,
                    PercentInEducationEmploymentOrTraining = ed.AllDest_Tot_Est_Current_Pct
                })),
            EnglandPercentage = englandDestinations.AllDest_Tot_Eng_Current_Pct
        };
    }
}
