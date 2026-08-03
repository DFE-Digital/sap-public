using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum QualificationType
{
    [Display(Name = "All qualifications")]
    AllQualifications,

    [Display(Name = "Academic qualifications")]
    AcademicQualifications,

    [Display(Name = "Vocational and technical qualifications")]
    VocationalAndTechnicalQualifications
}
