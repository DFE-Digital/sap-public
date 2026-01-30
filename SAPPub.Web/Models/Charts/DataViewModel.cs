namespace SAPPub.Web.Models.Charts
{
    public record DataViewModel
    {
        public required List<string> Labels { get; init; }

        public required List<double> Data { get; init; }

    }
}
