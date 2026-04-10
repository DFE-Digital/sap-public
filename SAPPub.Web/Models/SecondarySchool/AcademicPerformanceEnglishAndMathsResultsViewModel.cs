using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceEnglishAndMathsResultsViewModel : SecondarySchoolBaseViewModel
{
    public GcseGradeDataSelection? SelectedGrade { get; set; }

    public required DataViewModel AllGcseData { get; set; }

    public required DataOverTimeViewModel AllGcseOverTimeData { get; set; }

    public required SeriesViewModel BreakdownGcseData { get; set; }

    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public static AcademicPerformanceEnglishAndMathsResultsViewModel Map(EnglishAndMathsResultsModel englishAndMathsResultsModel, GcseGradeDataSelection selectedGrade)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(englishAndMathsResultsModel.LAName);

        var hasEstablishmentData = new[] 
        {
            englishAndMathsResultsModel.EstablishmentAll.CurrentYear,
            englishAndMathsResultsModel.EstablishmentAll.PreviousYear,
            englishAndMathsResultsModel.EstablishmentAll.TwoYearsAgo,
        }.All(d => d is double v && v != 0);

        var allGcseData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data = 
            [
                englishAndMathsResultsModel.EstablishmentAll.CurrentYear,
                englishAndMathsResultsModel.LocalAuthorityAll.CurrentYear,
                englishAndMathsResultsModel.EnglandAll.CurrentYear
            ],
        };

        var allGcseOverTimeData = new DataOverTimeViewModel
        {
            Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
            Datasets =
                [
                    new DatasetViewModel
                    {
                        Label = "School",
                        Data = [englishAndMathsResultsModel.EstablishmentAll.TwoYearsAgo, englishAndMathsResultsModel.EstablishmentAll.PreviousYear, englishAndMathsResultsModel.EstablishmentAll.CurrentYear],
                    },
                    new DatasetViewModel
                    {
                        Label = laAverageLabel,
                        Data = [englishAndMathsResultsModel.LocalAuthorityAll.TwoYearsAgo, englishAndMathsResultsModel.LocalAuthorityAll.PreviousYear, englishAndMathsResultsModel.LocalAuthorityAll.CurrentYear],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [englishAndMathsResultsModel.EnglandAll.TwoYearsAgo, englishAndMathsResultsModel.EnglandAll.PreviousYear, englishAndMathsResultsModel.EnglandAll.CurrentYear],
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
                        Data = [englishAndMathsResultsModel.EstablishmentGirls.CurrentYear, englishAndMathsResultsModel.EstablishmentBoys.CurrentYear]
                    },
                    new DataSeriesViewModel {
                        Label = laAverageLabel,
                        Data = [englishAndMathsResultsModel.LocalAuthorityGirls.CurrentYear, englishAndMathsResultsModel.LocalAuthorityBoys.CurrentYear]
                    },
                    new DataSeriesViewModel {
                        Label = "England average",
                        Data = [englishAndMathsResultsModel.EnglandGirls.CurrentYear, englishAndMathsResultsModel.EnglandBoys.CurrentYear]
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
            HasEstablishmentData = hasEstablishmentData.ToDisplayField(),
        };
    }
}
