using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Helpers;

namespace SAPPub.Core.ServiceModels;

public class EstablishmentMinimumServiceModel
{
    public string URN { get; set; } = string.Empty;

    public string EstablishmentName { get; set; } = string.Empty;

    public string EstablishmentNameClean => TextHelpers.CleanForUrl(EstablishmentName);

    public string LAId { get; set; } = string.Empty;

    public string LAName { get; set; } = string.Empty;

    public bool IsKS2 { get; set; }

    public bool IsKS4 { get; set; }

    public bool IsKS5 { get; set; }
}