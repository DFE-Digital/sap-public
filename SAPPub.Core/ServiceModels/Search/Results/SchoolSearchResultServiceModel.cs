using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.Common;

namespace SAPPub.Core.ServiceModels.Search.Results;

public enum SchoolSearchStatus
{
    Success = 0,
    InvalidPostcode,
    PostcodeNotFound,
    PostcodeServiceError,
    UnknownError
}

public record SchoolSearchResultsServiceModel
{
    public SchoolSearchStatus Status { get; set; }
    public required PagedResponse<SchoolSearchResultServiceModel> PagedResponse { get; init; }
}

public record SchoolSearchResultServiceModel
{
    public string? URN { get; init; }
    public string? EstablishmentName { get; init; }
    public string? Address { get; init; }
    public string? GenderName { get; init; }
    public string? ReligiousCharacterName { get; init; }
    public DateOnly? ClosedDate { get; set; }
    public EstablishmentStatus? EstablishmentStatus { get; set; }
    public bool IsKS4 { get; init; }
}
