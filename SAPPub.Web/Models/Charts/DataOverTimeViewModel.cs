namespace SAPPub.Web.Models.Charts;

public record DataOverTimeViewModel
{
    public required List<string> Labels { get; init; }

    public required List<DatasetViewModel> Datasets { get; init; }
}
