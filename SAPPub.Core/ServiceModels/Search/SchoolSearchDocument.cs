using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.ServiceModels.Search;

[ExcludeFromCodeCoverage]
public record SchoolSearchResults(int Count, IList<SchoolSearchDocument> Results);

[ExcludeFromCodeCoverage]
public record SchoolSearchDocument
{
    public string? URN { get; init; }
    public string? EstablishmentName { get; init; }
    public string? Address { get; init; }
    public int? TypeOfEstablishmentId { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public DateOnly? ClosedDate { get; set; }
    public int? StatusCode { get; set; }
    public bool IsKS2 { get; init; }
    public bool IsKS4 { get; init; }
    public bool IsKS5 { get; init; }
};
