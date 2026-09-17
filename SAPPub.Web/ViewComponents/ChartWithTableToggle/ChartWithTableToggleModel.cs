using SAPPub.Web.Constants;
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
    /// Optional three year average data, displayed as an additional chart and table
    /// beneath the "data over time" chart when supplied.
    /// </summary>
    public DataViewModel? ThreeYearAverageData { get; set; }

    /// <summary>
    /// Heading shown above the three year average chart/table.
    /// </summary>
    public string ThreeYearAverageHeading { get; set; } = Constants.Constants.ThreeYearAverageHeading;

    /// <summary>
    /// Description shown above the three year average chart/table.
    /// </summary>
    public string ThreeYearAverageDescription { get; set; } = Constants.Constants.ThreeYearAverageDescription;

    /// <summary>
    /// If scaled then uses numbers only (not as percentages), with a graph start of 80 and end of 120
    /// </summary>
    public required bool IsScaled { get; set; } = false;

    /// <summary>
    /// Affects display of table data only
    /// </summary>
    public required bool IsPercentageData { get; set; } = false;

}
