using System.ComponentModel.DataAnnotations;

namespace SAPPub.Core.Enums;

public enum PupilCharacteristic
{
    [Display(Name = "All pupils")]
    AllPupils,

    [Display(Name = "Disadvantaged pupils")]
    DisadvantagedPupils,

    [Display(Name = "Pupils with English as an additional language")]
    PupilsWithEnglishAsAnAdditionalLanguage,

    [Display(Name = "Girls and boys")]
    GirlsAndBoys,

    [Display(Name = "Pupils with different starting points")]
    PupilsWithDifferentStartingPoints,

    [Display(Name = "Non-mobile pupils")]
    NonMobilePupils
}
