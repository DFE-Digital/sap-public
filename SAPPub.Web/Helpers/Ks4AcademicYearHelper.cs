using SAPPub.Core.Enums;

namespace SAPPub.Web.Helpers;

public static class Ks4AcademicYearHelper
{
    public static string GetDisplayName(AcademicYearSelection academicYearSelection) => academicYearSelection switch
    {
        AcademicYearSelection.Current => "2025 to 2026",
        AcademicYearSelection.Previous => "2024 to 2025",
        AcademicYearSelection.Previous2 => "2023 to 2024",
        _ => throw new ArgumentOutOfRangeException(nameof(academicYearSelection), academicYearSelection, null)
    };
}
