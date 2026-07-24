using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum AcademicYearSelection
{
    [Display(Name = "2024 to 2025")]
    Current = 0,
    [Display(Name = "2023 to 2024")]
    Previous = 1,
    [Display(Name = "2022 to 2023")]
    Previous2 = 2,
}

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
