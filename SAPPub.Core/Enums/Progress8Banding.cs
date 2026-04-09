using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum Progress8Banding
{
    [Display(Name = "well above average")]
    WellAboveAverage,
    [Display(Name = "above average")]
    AboveAverage,
    [Display(Name = "average")]
    Average,
    [Display(Name = "below average")]
    BelowAverage,
    [Display(Name = "well below average")]
    WellBelowAverage,
    [Display(Name = "not available")]
    NotAvailable,
    [Display(Name = "suppressed")]
    Suppressed
}
