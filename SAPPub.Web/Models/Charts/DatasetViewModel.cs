namespace SAPPub.Web.Models.Charts;

public record DatasetViewModel
{
    public required string Label { get; set; }

    public required List<double> Data { get; init; }
}
