using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.ViewComponents.ChartWithTableToggle;

public class ChartWithTableToggleModel
{
    public required string IdPrefix { get; set; }

    public required string ChartPrefix { get; set; }

    public required DataViewModel CurrentData { get; set; }

    public required DataOverTimeViewModel OverTimeData { get; set; }
    public required bool HasEstablishmentData { get; set; }

}
