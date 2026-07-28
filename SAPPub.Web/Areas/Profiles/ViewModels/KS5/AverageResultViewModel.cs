using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class AverageResultViewModel
{
    public required DisplayField<double> EstablishmentPoints { get; init; }

    public required DisplayField<string> EstablishmentGrade { get; init; }

    public required DisplayField<double> LocalAuthorityPoints { get; init; }

    public required DisplayField<string> LocalAuthorityGrade { get; init; }

    public required DisplayField<double> EnglandPoints { get; init; }

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
