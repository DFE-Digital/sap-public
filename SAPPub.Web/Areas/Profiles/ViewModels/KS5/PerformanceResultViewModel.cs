using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class PerformanceResultViewModel
{
    public required DisplayField<CodedString> Grade { get; init; }

    public required DisplayField<CodedDouble> Points { get; init; }

    public static PerformanceResultViewModel Map(PerformanceResult? model)
    {
        return new PerformanceResultViewModel
        {
            Grade = model != null ? model.Grade.ToDisplayField() : CodedString.Empty.ToDisplayField(),
            Points = model != null ? model.Points.ToDisplayField() : CodedDouble.Empty.ToDisplayField()
        };
    }
}
