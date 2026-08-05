using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class AverageResultViewModel
{
    public required DisplayField<CodedDouble> EstablishmentPoints { get; init; }

    public required DisplayField<string> EstablishmentGrade { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityPoints { get; init; }

    public required DisplayField<string> LocalAuthorityGrade { get; init; }

    public required DisplayField<CodedDouble> EnglandPoints { get; init; }

    public required DisplayField<string> EnglandGrade { get; init; }

    public static AverageResultViewModel Map(AverageResultModel model)
    {
        return new AverageResultViewModel
        {
            EstablishmentPoints = model.Establishment.Points.ToDisplayField(),
            EstablishmentGrade = model.Establishment.Grade.ToDisplayField(),
            LocalAuthorityPoints = model.LocalAuthority.Points.ToDisplayField(),
            LocalAuthorityGrade = model.LocalAuthority.Grade.ToDisplayField(),
            EnglandPoints = model.England.Points.ToDisplayField(),
            EnglandGrade = model.England.Grade.ToDisplayField(),
        };
    }
}
