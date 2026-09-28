using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum KS2ProgressSubject
{
    [Display(Name = "Reading")]
    Reading = 1,

    [Display(Name = "Writing")]
    Writing = 2,

    [Display(Name = "Maths")]
    Maths = 3
}
