using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum GcseGradeDataSelection
{
    [Display(Name = "Grade 4 and above")]
    Grade4AndAbove = 4,
    [Display(Name = "Grade 5 and above")]
    Grade5AndAbove = 5
}
