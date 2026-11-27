using SAPPub.Web.Areas.Profiles.ViewModels.Overview;

namespace SAPPub.Web.Areas.Profiles.ViewModels;

public class KS2BandingsBaseViewModel : ProfileBaseViewModel
{
    public readonly static string Ks2SmallNumberOfPupilsWarningText = "These results are based on a small number of pupils. This means that a few pupils with particularly high or low results can have a greater impact on the overall result.";

    public required IReadOnlyList<ProgressBandingSummaryViewModel>? Ks2Bandings { get; init; }

    public string? Ks2SmallNumberOfPupilsWarning
    {
        get
        {
            if (Ks2Bandings is null)
            {
                return null;
            }

            var subjects = Ks2Bandings
                .Where(a => a.IsNotStatisticallySignificant)
                .Select(a => a.Subject.ToLowerInvariant())
                .ToList();

            if (subjects.Count == 0)
            {
                return null;
            }

            var subjectList = subjects.Count == 1
                ? subjects[0]
                : $"{string.Join(", ", subjects.Take(subjects.Count - 1))} and {subjects[^1]}";

            return $"{char.ToUpperInvariant(subjectList[0])}{subjectList[1..]}";
        }
    }
}