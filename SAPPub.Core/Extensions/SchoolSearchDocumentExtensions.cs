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
            TypeOfEstablishmentId = doc.TypeOfEstablishmentId,
            EstablishmentStatus = doc.StatusCode.ToStatus(),
            ClosedDate = doc.ClosedDate,
            IsKS2 = doc.IsKS2,
            IsKS4 = doc.IsKS4,
            IsKS5 = doc.IsKS5
        };
    }
}