using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums.KS5Qualifications
{
    public enum Level3
    {
        [Display(Name = "A levels")]
        ALevel = 1,
        [Display(Name = "Academic qualifications")]
        Academic = 2,
        [Display(Name = "Applied general qualifications")]
        AppliedGeneral = 3,
        [Display(Name = "Tech levels")]
        TechLevel = 4,
        [Display(Name = "Apprenticeship")]
        Apprenticeship = 5
    }

    public enum Level2
    {
        [Display(Name = "Technical certificates")]
        TechCert = 1,
        [Display(Name = "Apprenticeship")]
        Apprenticeship = 2
    }
}
