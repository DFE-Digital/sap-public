using SAPPub.Core.Entities;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class EnglishAndMathsResultsModel
{
    public required string Urn { get; init; }

    public required string SchoolName { get; init; }

    public required string? LAName { get; set; }
    
    public required RelativeYearValues<double?> EstablishmentAll { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityAll { get; init; }

    public required RelativeYearValues<double?> EnglandAll { get; init; }

    public required RelativeYearValues<double?> EstablishmentBoys { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityBoys { get; init; }

    public required RelativeYearValues<double?> EnglandBoys { get; init; }

    public required RelativeYearValues<double?> EstablishmentGirls { get; init; }

    public required RelativeYearValues<double?> LocalAuthorityGirls { get; init; }

    public required RelativeYearValues<double?> EnglandGirls { get; init; }
}
