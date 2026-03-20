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
