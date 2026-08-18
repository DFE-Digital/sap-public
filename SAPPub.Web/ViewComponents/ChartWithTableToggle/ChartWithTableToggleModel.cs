using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.ViewComponents.ChartWithTableToggle;

public class ChartWithTableToggleModel
{
    public required string IdPrefix { get; set; }

    public required string ChartPrefix { get; set; }

    public required DataViewModel CurrentData { get; set; }

    public required DataOverTimeViewModel OverTimeData { get; set; }
    public required bool HasEstablishmentData { get; set; }

    /// <summary>
    /// If scaled then uses numbers only (not as percentages), with a graph start of 80 and end of 120
    /// </summary>
    public required bool IsScaled { get; set; } = false;

    /// <summary>
    /// Affects display of table data only
    /// </summary>
    public required bool IsPercentageData { get; set; } = false;

}
