namespace SAPPub.Web.Models.Charts;

public class GcseGradesOverTimeViewModel
{
    public required List<string> Labels { get; set; }

    public required List<string> Years { get; set; }

    public required List<double> School { get; set; }

    public required List<double> LocalAuthority { get; set; }

    public required List<double> England { get; set; }
}
