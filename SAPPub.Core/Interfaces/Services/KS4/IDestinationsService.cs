using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Services.KS4;

public interface IDestinationsService
{
    DestinationsDetails GetDestinationsDetails(string urn);
}
