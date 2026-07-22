using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class ProgressScoreViewModel
{
    public required DisplayField<double> Score { get; init; }

    public required DisplayField<string> BandingRating { get; init; }

    public required DisplayField<double> ConfidenceLevelUpper { get; init; }

    public required DisplayField<double> ConfidenceLevelLower { get; init; }

    public required DisplayField<double> EnglandAverageScore { get; init; }

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
            Progress8BandingContextDescription = AttainmentHelper.EstablishmentProgress8BandingContextStatement(model.BandingRating)
        };        
    }
}
