namespace SAPPub.Web.Models.Charts
{
    public record DataSeriesViewModel
    {
        public required string Label { get; set; }

        public required List<double> Data { get; init; }
    }
}
