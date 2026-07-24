using SAPPub.Core.Enums;

namespace SAPPub.Web.Areas.Profiles.Helpers;

public static class AcademicYearSelectionExtensions
{
    public static string ToRouteSegment(this AcademicYearSelection year)
    {
        return year switch
        {
            AcademicYearSelection.Current => "current",
            AcademicYearSelection.Previous => "previous",
            AcademicYearSelection.Previous2 => "previous2",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public static AcademicYearSelection FromRouteSegment(string routeSegment)
    {
        return routeSegment switch
        {
            "current" => AcademicYearSelection.Current,
            "previous" => AcademicYearSelection.Previous,
            "previous2" => AcademicYearSelection.Previous2,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
