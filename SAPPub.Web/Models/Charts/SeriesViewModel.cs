namespace SAPPub.Web.Models.Charts
{
    public record SeriesViewModel
    {
        public required List<string> Labels { get; init; }

        public required List<DataSeriesViewModel> Datasets { get; init; }
    }
}
