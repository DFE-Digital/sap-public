namespace SAPPub.Web.Models.Charts;

public class GcseDataViewModel
{
    public required List<string> Labels { get; set; }

    public required List<double> GcseData { get; set; }

    public required string ChartTitle { get; set; }

    public List<string>? HeaderLabels { get; set; }

}
