using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Models.Charts;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareAcademicPerformanceEnglishAndMathsResultsViewModel : CompareSecondarySchoolBaseViewModel
{
    public required DataViewModel AllGcseData { get; set; }

    public static CompareAcademicPerformanceEnglishAndMathsResultsViewModel Map(
        List<string> urns,
        EnglishAndMathsComparisionResultsModel englishAndMathsComparisionResultsModel)
    {
        var orderedResults = englishAndMathsComparisionResultsModel.Establishments.OrderBy(x => x.SchoolName).ToList();

        var allGcseData = new DataViewModel
        {
            Labels = [.. orderedResults.Select(x => x.SchoolName).Append(EnglandAverage)],
            Data = [.. orderedResults.Select(x => x.EstablishmentData.CurrentYear).Append(englishAndMathsComparisionResultsModel.EnglandAverage.CurrentYear)],
            BackgroundColors = [.. Enumerable.Repeat(EstablishmentChartColour, orderedResults.Count), EnglandAverageChartColour]
        };

        return new CompareAcademicPerformanceEnglishAndMathsResultsViewModel
        {
            URNs = urns,
            AllGcseData = allGcseData,        
        };
    }
}
