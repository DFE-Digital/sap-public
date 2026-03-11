using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceEnglishAndMathsResultsViewModel : SecondarySchoolBaseViewModel
{
    public GcseGradeDataSelection? SelectedGrade { get; set; }

    public required DataViewModel AllGcseData { get; set; }

    public required DataOverTimeViewModel AllGcseOverTimeData { get; set; }

    public required SeriesViewModel BreakdownGcseData { get; set; }

    public static AcademicPerformanceEnglishAndMathsResultsViewModel Map(EnglishAndMathsResultsModel englishAndMathsResultsModel, GcseGradeDataSelection selectedGrade)
    {
        var laAverageLabel = string.IsNullOrEmpty(englishAndMathsResultsModel.LAName) ? "Local Authority average" : $"{englishAndMathsResultsModel.LAName} average";

        var allGcseData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data = [englishAndMathsResultsModel.EstablishmentAll.CurrentYear ?? 0, englishAndMathsResultsModel.LocalAuthorityAll.CurrentYear ?? 0, englishAndMathsResultsModel.EnglandAll.CurrentYear ?? 0],
        };

        var allGcseOverTimeData = new DataOverTimeViewModel
        {
            Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
            Datasets =
                [
                    new DatasetViewModel
                    {
                        Label = "School",
                        Data = [englishAndMathsResultsModel.EstablishmentAll.TwoYearsAgo ?? 0, englishAndMathsResultsModel.EstablishmentAll.PreviousYear ?? 0, englishAndMathsResultsModel.EstablishmentAll.CurrentYear ?? 0],
                    },
                    new DatasetViewModel
                    {
                        Label = laAverageLabel,
                        Data = [englishAndMathsResultsModel.LocalAuthorityAll.TwoYearsAgo ?? 0, englishAndMathsResultsModel.LocalAuthorityAll.PreviousYear ?? 0, englishAndMathsResultsModel.LocalAuthorityAll.CurrentYear ?? 0],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [englishAndMathsResultsModel.EnglandAll.TwoYearsAgo ?? 0, englishAndMathsResultsModel.EnglandAll.PreviousYear ?? 0, englishAndMathsResultsModel.EnglandAll.CurrentYear ?? 0],
                    }
                ],
        };

        var breakdownGcseData = new SeriesViewModel
        {
            Labels = ["Girls", "Boys"],
            Datasets =
                [
                    new DataSeriesViewModel {
                        Label = "School",
                        Data = [englishAndMathsResultsModel.EstablishmentGirls.CurrentYear ?? 0, englishAndMathsResultsModel.EstablishmentBoys.CurrentYear ?? 0]
                    },
                    new DataSeriesViewModel {
                        Label = laAverageLabel,
                        Data = [englishAndMathsResultsModel.LocalAuthorityGirls.CurrentYear ?? 0, englishAndMathsResultsModel.LocalAuthorityBoys.CurrentYear ?? 0]
                    },
                    new DataSeriesViewModel {
                        Label = "England average",
                        Data = [englishAndMathsResultsModel.EnglandGirls.CurrentYear ?? 0, englishAndMathsResultsModel.EnglandBoys.CurrentYear ?? 0]
                    },
                ],
        };


        return new AcademicPerformanceEnglishAndMathsResultsViewModel
        {
            URN = englishAndMathsResultsModel.Urn,
            SchoolName = englishAndMathsResultsModel.SchoolName,
            SelectedGrade = selectedGrade,
            AllGcseData = allGcseData,
            AllGcseOverTimeData = allGcseOverTimeData,
            BreakdownGcseData = breakdownGcseData,
        };
    }
}
