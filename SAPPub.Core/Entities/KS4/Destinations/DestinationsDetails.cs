namespace SAPPub.Core.Entities.KS4.Destinations;

public record DestinationsDetails
{
    public required string Urn { get; init; }

    public required string SchoolName { get; init; }

    public required string LocalAuthorityName { get; init; }

    public required RelativeYearValues<double?> SchoolAll { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityAll { get; init; }

    public required RelativeYearValues<double?> EnglandAll { get; init; }
}
