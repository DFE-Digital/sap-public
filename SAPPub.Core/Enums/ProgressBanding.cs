using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum ProgressBanding
{
    [Display(Name = "well above average")]
    WellAboveAverage =  1,

    [Display(Name = "above average")]
    AboveAverage = 2,
    
    [Display(Name = "average")]
    Average = 3,
    
    [Display(Name = "below average")]
    BelowAverage = 4,
    
    [Display(Name = "well below average")]
    WellBelowAverage = 5
}
