namespace SAPPub.Web.Models.Charts
{
    public record SeriesCodedDoubleViewModel
    {
        public required List<string> Labels { get; init; }

        public required List<DatasetCodedDoubleViewModel> Datasets { get; init; }
    }
}
