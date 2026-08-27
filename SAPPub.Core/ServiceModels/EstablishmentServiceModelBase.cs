using SAPPub.Core.Enums;

namespace SAPPub.Core.ServiceModels;

public abstract class EstablishmentServiceModelBase
{
    public TypeOfEstablishment TypeOfEstablishment { get; set; }

    public bool IsSpecialSchool => TypeOfEstablishment is
        TypeOfEstablishment.CommunitySpecialSchool or
        TypeOfEstablishment.NonMaintainedSpecialSchool or
        TypeOfEstablishment.OtherIndependentSpecialSchool or
        TypeOfEstablishment.FoundationSpecialSchool or
        TypeOfEstablishment.AcademySpecialSponsorLed or
        TypeOfEstablishment.FreeSchoolsSpecial or
        TypeOfEstablishment.AcademySpecialConverter;

    public bool IsIndependentSchool => TypeOfEstablishment is
        TypeOfEstablishment.OtherIndependentSchool or
        TypeOfEstablishment.OtherIndependentSpecialSchool;

}
