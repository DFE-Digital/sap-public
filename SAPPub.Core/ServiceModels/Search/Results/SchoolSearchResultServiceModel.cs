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
    public IList<SchoolSearchResultServiceModel> SchoolSearchResults { get; init; } = new List<SchoolSearchResultServiceModel>();
    public int Count { get; init; }
    public SchoolSearchStatus Status { get; set; }
}

public record SchoolSearchResultServiceModel
{
    public string? URN { get; init; }
    public string? EstablishmentName { get; init; }
    public string? Address { get; init; }
    public string? GenderName { get; init; }
    public string? ReligiousCharacterName { get; init; }
    public double? Distance { get; init; }
    public DateOnly? ClosedDate { get; set; }
    public int? StatusCode { get; set; }
}
