using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class EnglishMathsScoreViewModel
{
    public required DisplayField<double> NumberOfStudents { get; init; }

    public required DisplayField<double> SchoolOrCollege { get; init; }

    public required DisplayField<double> LaAverage { get; init; }

    public required DisplayField<double> EnglandAverage { get; init; }

    public static EnglishMathsScoreViewModel Map(EnglishMathsScoreModel model)
    {
        return new EnglishMathsScoreViewModel
        {
            NumberOfStudents = model.NumberOfStudents.ToDisplayField(),
            EnglandAverage = model.EnglandAverage.ToDisplayField(),
            LaAverage = model.LaAverage.ToDisplayField(),
            SchoolOrCollege = model.SchoolOrCollege.ToDisplayField()
        };
    }
}
