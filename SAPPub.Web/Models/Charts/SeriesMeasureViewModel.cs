namespace SAPPub.Web.Models.Charts
{
    public record SeriesMeasureViewModel
    {
        public string TableId { get; init; } = string.Empty;
        public string? TableHeader { get; init; }
        public required List<string> Labels { get; init; }

        public required List<DatasetMeasureViewModel> Datasets { get; init; }
    }
}
