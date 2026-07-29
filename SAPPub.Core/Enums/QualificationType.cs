using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum QualificationType
{
    [Display(Name = "All Qualifications")]
    AllQualifications,

    [Display(Name = "Academic Qualifications")]
    AcademicQualifications,

    [Display(Name = "Vocational and technical qualifications")]
    VocationalAndTechnicalQualifications
}
