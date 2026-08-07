using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class ProgressScoreViewModel
{
    public required DisplayField<CodedDouble> Score { get; init; }

    public required DisplayField<CodedString> BandingRating { get; init; }

    public required DisplayField<CodedDouble> ConfidenceLevelUpper { get; init; }

    public required DisplayField<CodedDouble> ConfidenceLevelLower { get; init; }

    public required DisplayField<CodedDouble> EnglandAverageScore { get; init; }

    public required DisplayField<string> Progress8BandingContextDescription { get; init; }

    public static ProgressScoreViewModel Map(ProgressScoreModel model)
    {
        return new ProgressScoreViewModel
        {
            Score = model.Score.ToDisplayField(),
            BandingRating = model.BandingRating.ToDisplayField(),
            ConfidenceLevelUpper = model.ConfidenceLevelUpper.ToDisplayField(),
            ConfidenceLevelLower = model.ConfidenceLevelLower.ToDisplayField(),
            EnglandAverageScore = model.EnglandAverageScore.ToDisplayField(),
            Progress8BandingContextDescription = AttainmentHelper.EstablishmentProgress8BandingContextStatement(model.BandingRating.Value)
        };        
    }
}
