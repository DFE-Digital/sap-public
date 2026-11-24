using SAPPub.Core.Attributes;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities;

[ExcludeFromCodeCoverage]
public class EstablishmentMinimum : EstablishmentServiceModelBase
{

    public static EstablishmentMinimumServiceModel MapToServiceModel(Establishment e)
    {
        return new()
        {
            URN = e.URN,
            EstablishmentName = e.EstablishmentName,
            LAId = e.LAId,
            LAName = e.LAName,
            IsKS2 = e.IsKS2,
            IsKS4 = e.IsKS4,
            IsKS5 = e.IsKS5,
            Website = e.Website,
            LAESTAB = $"{e.LAId}{e.EstablishmentNumber}",
            TypeOfEstablishment = e.TypeOfEstablishmentId is int id && Enum.IsDefined(typeof(TypeOfEstablishment), id)
                ? (TypeOfEstablishment)e.TypeOfEstablishmentId
                : TypeOfEstablishment.Unknown,
        };
    }
}
