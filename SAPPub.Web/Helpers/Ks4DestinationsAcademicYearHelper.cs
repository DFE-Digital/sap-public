using SAPPub.Core.Enums;

namespace SAPPub.Web.Helpers;

/// <summary>
/// Provides KS4 destinations-specific academic year display names.
/// Destinations data is lagged by 2 years (pupils who finished year 11 in the specified year).
/// </summary>
public static class Ks4DestinationsAcademicYearHelper
{
    public static string GetDisplayName(AcademicYearSelection academicYearSelection) =>
        academicYearSelection switch
        {
            AcademicYearSelection.Current => "2023 to 2024",
            AcademicYearSelection.Previous => "2022 to 2023",
            AcademicYearSelection.Previous2 => "2021 to 2022",
            _ => throw new ArgumentOutOfRangeException(nameof(academicYearSelection), academicYearSelection, null)
        };
}
