using SAPPub.Core.Enums;
using SAPPub.Core.Helpers;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.Search;

public class SearchResult
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string EstablishmentNameClean => TextHelpers.CleanForUrl(EstablishmentName);
    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? GenderName { get; set; }

    public int? TypeOfEstablishmentId { get; set; }
    public string? EstablishmentType { get; set; }

    public required DisplayField<DateOnly> ClosedDate { get; set; }
    public EstablishmentStatus? EstablishmentStatus { get; set; }
    public bool IsSchoolClosed => EstablishmentStatus == Core.Enums.EstablishmentStatus.Closed;
    public bool IsKS2 { get; init; }
    public bool IsKS4 { get; init; }
    public bool IsKS5 { get; init; }

    public string? PhaseDescription { get; set; }

    public static SearchResult FromServiceModel(SchoolSearchResultServiceModel serviceModel)
    {
        var phaseDescription = serviceModel.IsKS2 && serviceModel.IsKS4 && serviceModel.IsKS5 ? "All-through" :
            serviceModel.IsKS2 ? "Primary" :
            serviceModel.IsKS4 ? "Secondary" :
            serviceModel.IsKS5 ? "16 to 19" : null;

        var estabType = serviceModel.TypeOfEstablishmentId switch
        {
            1 or  2 or  3 or 5 => "Maintained school",
            28 or 34 or 35 or 40 or 41 or 45 or 46 => "Academy",
            18 or 21 or 39 or 56 => "College",
            6 or 11 => "Independent school",
            7 or 8 or 10 or 12 or 33 or 36 or 44 => "Special school",
            _ => null
        };


        return new SearchResult
        {
            URN = serviceModel.URN?.ToString() ?? string.Empty,
            EstablishmentName = serviceModel.EstablishmentName ?? string.Empty,
            Address = serviceModel.Address ?? string.Empty,
            TypeOfEstablishmentId = serviceModel.TypeOfEstablishmentId,
            EstablishmentStatus = serviceModel.EstablishmentStatus,
            ClosedDate = serviceModel.ClosedDate.ToDisplayField(),
            IsKS2 = serviceModel.IsKS2,
            IsKS4 = serviceModel.IsKS4,
            IsKS5 = serviceModel.IsKS5,
            PhaseDescription = phaseDescription, 
            EstablishmentType = estabType
        };
    }
}
