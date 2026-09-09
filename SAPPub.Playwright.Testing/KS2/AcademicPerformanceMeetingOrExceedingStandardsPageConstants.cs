namespace SAPPub.Playwright.Testing.KS2.Performance.MeetingOrExceedingStandards;

public class PageConstants
{
    public static readonly IReadOnlyDictionary<string, string> ContentIds =
            new Dictionary<string, string>()
            {
                ["currentYearShowAsTableBtn"] = "#mes-current-year-show-btn",
                ["currentYearChartContainer"] = "#mes-current-year-chart-container",
                ["currentYearTableContainer"] = "#mes-current-year-table-container",
                ["showDataOverTimeBtn"] = "#mes-show-data-over-time-btn",
                ["dataOverTimeChartContainer"] = "#mes-data-over-time-chart-container",
                ["dataOverTimeTableContainer"] = "#mes-data-over-time-table-container",
                ["dataOverTimeChartLegend"] = "#mes-data-overtime-chart-legend",
                ["dataOverTimeShowAsTableBtn"] = "#mes-data-over-time-show-btn",
                ["showCurrentDataBtn"] = "#mes-show-current-data-btn",
                ["dataOverTimeTable"] = "#mes-data-overtime-table",
                ["exsDataOverTimeTable"] = "#exs-data-overtime-table",
                ["girls-boys-table"] = "#girls-boys-table",
                ["eal-table"] = "#eal-table",
                ["non-mobile-table"] = "#nonmobile-pupils-table",
                ["disadvantaged-pupils-table"] = "#disadvantaged-pupils-table"
            };
}
