using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class PerformanceDataViewModel
{
    public required DisplayField<CodedDouble> NumberOfStudents { get; init; }

    public required DisplayField<CodedDouble> ProgressScore { get; init; }

    public required DisplayField<string> ConfidenceInterval { get; init; }

    public required DisplayField<CodedDouble> Points { get; init; }

    public required DisplayField<CodedString> Grade { get; init; }
    
    public static PerformanceDataViewModel Map(PerformanceData model)
    {
        return new PerformanceDataViewModel
        {
            NumberOfStudents = model.NumberOfStudents.ToDisplayField(),
            ProgressScore = model.ProgressScore.ToDisplayField(),
            Points = model.Result.Points.ToDisplayField(),
            Grade = model.Result.Grade.ToDisplayField(),
            ConfidenceInterval = model.ConfidenceLevelLower.HasValue && model.ConfidenceLevelUpper.HasValue 
                ? $"{model.ConfidenceLevelLower.Value} to {model.ConfidenceLevelUpper.Value}".ToDisplayField() 
                : DisplayField<string>.NotAvailable(),
        };
    }
}
