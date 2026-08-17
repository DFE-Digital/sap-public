using SAPPub.Core.Attributes;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities;

[ExcludeFromCodeCoverage]
public class EstablishmentMinimum
{
    public string URN { get; set; } = string.Empty;

    public string EstablishmentName { get; set; } = string.Empty;

    public string LAId { get; set; } = string.Empty;

    public string LAName { get; set; } = string.Empty;

    [DbColumnName("ISKS2")]
    public bool IsKS2 { get; set; }

    [DbColumnName("ISKS4")]
    public bool IsKS4 { get; set; }

    [DbColumnName("ISKS5")]
    public bool IsKS5 { get; set; }

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
        };
    }
}
