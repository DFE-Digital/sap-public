namespace SAPPub.Web.Models.Charts;

public class GcseDataViewModel
{
    public required List<string> Lables { get; set; }

    public required List<double> GcseData { get; set; }

    public required string ChartTitle { get; set; }

}
