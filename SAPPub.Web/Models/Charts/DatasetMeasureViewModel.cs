using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Models.Charts;

public record DatasetMeasureViewModel
{
    public string? Label { get; set; }

    public required List<Measure> Data { get; init; }
}
