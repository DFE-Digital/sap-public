using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class EnglishMathsScoreViewModel
{
    public required DisplayField<CodedDouble> NumberOfStudents { get; init; }

    public required DisplayField<CodedDouble> SchoolOrCollege { get; init; }

    public required DisplayField<CodedDouble> LaAverage { get; init; }

    public required DisplayField<CodedDouble> EnglandAverage { get; init; }

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
