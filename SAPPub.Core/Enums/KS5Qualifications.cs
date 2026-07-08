using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Enums.KS5Qualifications
{
    public enum Level3
    {
        [Display(Name = "A level")]
        ALevel = 1,
        [Display(Name = "Academic")]
        Academic = 2,
        [Display(Name = "Applied General")]
        AppliedGeneral = 3,
        [Display(Name = "Tech Level")]
        TechLevel = 4,
        [Display(Name = "Apprenticeship")]
        Apprenticeship = 5
    }

    public enum Level2
    {
        [Display(Name = "Technical Certificate")]
        TechCert = 1,
        [Display(Name = "Apprenticeship")]
        Apprenticeship = 2
    }


}
