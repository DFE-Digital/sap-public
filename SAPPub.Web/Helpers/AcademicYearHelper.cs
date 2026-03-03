using SAPPub.Core.Enums;

namespace SAPPub.Web.Helpers;

public class AcademicYearHelper
{
    public static string GetAcademicYearText(AcademicYearSelection academicYear)
    {
        return academicYear switch
        {
            AcademicYearSelection.Current => "2024 to 2025",
            AcademicYearSelection.Previous => "2023 to 2024",
            AcademicYearSelection.Previous2 => "2022 to 2023",
            _=> string.Empty,
        };
    }
}
