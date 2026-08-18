using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class AdditionalDataViewModel
{
    public required DisplayField<CodedDouble> TotalNoOfStudentsIncludedInThisMeasure { get; init; }

    public required DisplayField<CodedDouble> EstablishmentPoints { get; init; }

    public required DisplayField<CodedString> EstablishmentGrade { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityPoints { get; init; }

    public required DisplayField<CodedString> LocalAuthorityGrade { get; init; }

    public required DisplayField<CodedDouble> EnglandPoints { get; init; }

    public required DisplayField<CodedString> EnglandGrade { get; init; }

    public static AdditionalDataViewModel Map(AdditionalDataModel model)
    {
        return new AdditionalDataViewModel
        {
            TotalNoOfStudentsIncludedInThisMeasure = model.TotalNoOfStudentsIncludedInThisMeasure.ToDisplayField(),
            EstablishmentPoints = model.Establishment.Points.ToDisplayField(),
            EstablishmentGrade = model.Establishment.Grade.ToDisplayField(),
            LocalAuthorityPoints = model.LocalAuthority.Points.ToDisplayField(),
            LocalAuthorityGrade = model.LocalAuthority.Grade.ToDisplayField(),
            EnglandPoints = model.England.Points.ToDisplayField(),
            EnglandGrade = model.England.Grade.ToDisplayField(),
        };
    }
}
