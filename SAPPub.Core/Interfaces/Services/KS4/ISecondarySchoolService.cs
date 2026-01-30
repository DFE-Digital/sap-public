using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Core.Interfaces.Services.KS4;

public interface ISecondarySchoolService
{
    DestinationsDetails GetDestinationsDetails(string urn);
}
