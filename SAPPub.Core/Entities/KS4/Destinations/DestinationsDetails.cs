namespace SAPPub.Core.Entities.KS4.Destinations;

public record DestinationsDetails
{
    public required string Urn { get; init; }

    public required string SchoolName { get; init; }

    public required string LocalAuthorityName { get; init; }

    public required RelativeYearValues<double?> SchoolAll { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityAll { get; init; }

    public required RelativeYearValues<double?> EnglandAll { get; init; }

    public required RelativeYearValues<double?> SchoolEducation { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityEducation { get; init; }

    public required RelativeYearValues<double?> EnglandEducation { get; init; }

    public required RelativeYearValues<double?> SchoolEmployment { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityEmployment { get; init; }

    public required RelativeYearValues<double?> EnglandEmployment { get; init; }

    public required RelativeYearValues<double?> SchoolApprentice { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityApprentice { get; init; }

    public required RelativeYearValues<double?> EnglandApprentice { get; init; }
}
