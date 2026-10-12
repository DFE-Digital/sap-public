namespace SAPPub.Core.Enums;

public static class TypeOfEstablishmentExtensions
{
    extension(TypeOfEstablishment type)
    {
        public bool IsSpecialSchool => type is
            TypeOfEstablishment.CommunitySpecialSchool or
            TypeOfEstablishment.NonMaintainedSpecialSchool or
            TypeOfEstablishment.OtherIndependentSpecialSchool or
            TypeOfEstablishment.FoundationSpecialSchool or
            TypeOfEstablishment.AcademySpecialSponsorLed or
            TypeOfEstablishment.FreeSchoolsSpecial or
            TypeOfEstablishment.AcademySpecialConverter;

        public bool IsIndependentSchool => type is
            TypeOfEstablishment.OtherIndependentSchool or
            TypeOfEstablishment.OtherIndependentSpecialSchool;
    }
}