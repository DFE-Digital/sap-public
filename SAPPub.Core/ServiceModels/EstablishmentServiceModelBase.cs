using SAPPub.Core.Enums;

namespace SAPPub.Core.ServiceModels;

public abstract class EstablishmentServiceModelBase
{
    public TypeOfEstablishment TypeOfEstablishment { get; set; }

    public bool IsSpecialSchool => TypeOfEstablishment.IsSpecialSchool;

    public bool IsIndependentSchool => TypeOfEstablishment.IsIndependentSchool;
}
