using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum AcademicYearSelection
{
    [Display(Name = "2024 to 2025")]
    AY_2024_2025 = 2024,
    [Display(Name = "2023 to 2024")]
    AY_2023_2024 = 2023,
    [Display(Name = "2022 to 2023")]
    AY_2022_2023 = 2022,
}
