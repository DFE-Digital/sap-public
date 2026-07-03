using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Models.Charts;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareAcademicPerformanceEnglishAndMathsResultsViewModel : CompareSecondarySchoolBaseViewModel
{
    public required DataViewModel AllGcseData { get; set; }

    public required DataOverTimeViewModel AllGcseOverTimeData { get; set; }

    public static CompareAcademicPerformanceEnglishAndMathsResultsViewModel Map(
        List<string> urns,
        EnglishAndMathsComparisionResultsModel englishAndMathsComparisionResultsModel,
        IEnumerable<EstablishmentServiceModel> establishments)
    {
        var orderedResults = englishAndMathsComparisionResultsModel.Establishments.OrderBy(x => x.SchoolName).ToList();

        var allGcseData = new DataViewModel
        {
            Labels = [.. orderedResults.Select(x => x.SchoolName).Append(EnglandAverage)],
            Data = [.. orderedResults.Select(x => x.EstablishmentData.CurrentYear).Append(englishAndMathsComparisionResultsModel.EnglandAverage.CurrentYear)],
            BackgroundColors = [.. Enumerable.Repeat(EstablishmentChartColour, orderedResults.Count), EnglandAverageChartColour]
        };

        var allGcseOverTimeData = new DataOverTimeViewModel
        {
            Labels = [.. orderedResults.Select(x => x.SchoolName).Append(EnglandAverage)],
            Datasets =
                [
                    new DatasetViewModel
                    {
                        Label = "2022 to 2023",
                        Data = [.. orderedResults.Select(x => x.EstablishmentData.TwoYearsAgo).Append(englishAndMathsComparisionResultsModel.EnglandAverage.TwoYearsAgo)],
                    },
                    new DatasetViewModel
                    {
                        Label = "2023 to 2024",
                        Data = [.. orderedResults.Select(x => x.EstablishmentData.PreviousYear).Append(englishAndMathsComparisionResultsModel.EnglandAverage.PreviousYear)],
                    },
                    new DatasetViewModel
                    {
                        Label = "2024 to 2025",
                        Data = [.. orderedResults.Select(x => x.EstablishmentData.CurrentYear).Append(englishAndMathsComparisionResultsModel.EnglandAverage.CurrentYear)],
                    }
                ],
        };

        return new CompareAcademicPerformanceEnglishAndMathsResultsViewModel
        {
            URNs = urns,
            ListContainsSpecialSchool = establishments.Any(e => e.IsSpecialSchool),
            AllGcseData = allGcseData,
            AllGcseOverTimeData = allGcseOverTimeData,
        };
    }
}
