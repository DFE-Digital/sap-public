using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.Search;

public class SearchResult
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? GenderName { get; set; }
    public required DisplayField<DateOnly> ClosedDate { get; set; }
    public int? StatusCode { get; set; }
    public bool IsSchoolClosed => StatusCode == 2;

    public static SearchResult FromServiceModel(SchoolSearchResultServiceModel serviceModel)
    {
        return new SearchResult
        {
            URN = serviceModel.URN?.ToString() ?? string.Empty,
            EstablishmentName = serviceModel.EstablishmentName ?? string.Empty,
            Address = serviceModel.Address ?? string.Empty,
            ReligiousCharacter = serviceModel.ReligiousCharacterName ?? string.Empty,
            GenderName = serviceModel.GenderName ?? string.Empty,
            StatusCode = serviceModel.StatusCode,
            ClosedDate = serviceModel.ClosedDate.ToDisplayField()
        };
    }
}
