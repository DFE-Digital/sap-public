using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.ServiceModels.PostcodeSearch;

[ExcludeFromCodeCoverage]
public record SchoolSearchResults(int Count, IList<SchoolSearchDocument> Results);

[ExcludeFromCodeCoverage]
public record SchoolSearchDocument
{
    public string? URN { get; init; }
    public string? EstablishmentName { get; init; }
    public string? Address { get; init; }
    public string? GenderName { get; init; }
    public string? ReligiousCharacterName { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public DateOnly? ClosedDate { get; set; }
    public int? StatusCode { get; set; }
};
