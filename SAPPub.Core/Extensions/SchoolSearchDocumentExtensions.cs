using SAPPub.Core.ServiceModels.Search;
using SAPPub.Core.ServiceModels.Search.Results;

namespace SAPPub.Core.Extensions;

public static class SchoolSearchDocumentExtensions
{
    public static SchoolSearchResultServiceModel ToSchoolSearchResult(this SchoolSearchDocument doc)
    {
        return new SchoolSearchResultServiceModel
        {
            URN = doc.URN,
            EstablishmentName = doc.EstablishmentName,
            Address = doc.Address,
            GenderName = doc.GenderName,
            ReligiousCharacterName = doc.ReligiousCharacterName,
            EstablishmentStatus = doc.StatusCode.ToStatus(),
            ClosedDate = doc.ClosedDate,
            IsKS4 = doc.IsKS4
        };
    }
}