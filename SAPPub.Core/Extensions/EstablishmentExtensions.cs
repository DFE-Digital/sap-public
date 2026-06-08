using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Core.Extensions;

public static class EstablishmentExtensions
{
    public static SchoolSearchDocument ToSchoolSearchDocument(this Establishment e)
    {
        return new SchoolSearchDocument
        {
            URN = e.URN,
            EstablishmentName = e.EstablishmentName,
            Address = e.Address,
            GenderName = e.GenderName,
            ReligiousCharacterName = e.ReligiousCharacterName,
            ClosedDate = e.ClosedDate.ToDateOnly(),
            StatusCode = e.StatusCode
        };
    }
}