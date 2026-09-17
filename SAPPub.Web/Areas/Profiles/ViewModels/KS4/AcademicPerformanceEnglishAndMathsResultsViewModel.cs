using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceEnglishAndMathsResultsViewModel : BaseViewModel
{
    public GcseGradeDataSelection? SelectedGrade { get; set; }

    public required DataViewModel AllGcseData { get; set; }

    public required DataOverTimeViewModel AllGcseOverTimeData { get; set; }

    public required SeriesViewModel BreakdownGcseData { get; set; }

    public required SeriesMeasureViewModel BreakdownDisadvantaged { get; set; }

    public required SeriesMeasureViewModel BreakdownNonDisadvantaged { get; set; }

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

        var disadvantagedBreakdownData = new SeriesMeasureViewModel
        {
            TableId = "breakdown-disadvantaged-table",
            TableHeader = "Pupil group (disadvantaged)",
            Labels = [$"Percentage who achieved {selectedGrade.GetDisplayName()} in English and maths"],
            Datasets =
                [
                    new DatasetMeasureViewModel {
                        Label = "School",
                        Data = [ new Measure { Value = englishAndMathsResultsModel.EstablishmentDisadvantaged.CurrentYear, Unit = DataUnit.Percentage }]
                    },
                    new DatasetMeasureViewModel {
                        Label = laAverageLabel,
                        Data = [ new Measure { Value = englishAndMathsResultsModel.LocalAuthorityDisadvantaged.CurrentYear, Unit = DataUnit.Percentage }]
                    },
                    new DatasetMeasureViewModel {
                        Label = "England average",
                        Data = [ new Measure { Value = englishAndMathsResultsModel.EnglandDisadvantaged.CurrentYear, Unit = DataUnit.Percentage }]
                    },
                ],
        };

        var nonDisadvantagedBreakdownData = new SeriesMeasureViewModel
        {
            TableId = "breakdown-non-disadvantaged-table",
            TableHeader = "Pupil group (non-disadvantaged)",
            Labels = [$"Percentage who achieved {selectedGrade.GetDisplayName()} in English and maths"],
            Datasets =
                    [
                        new DatasetMeasureViewModel {
                            Label = laAverageLabel,
                            Data = [ new Measure { Value = englishAndMathsResultsModel.LocalAuthorityNonDisadvantaged.CurrentYear, Unit = DataUnit.Percentage }]
                        },
                        new DatasetMeasureViewModel {
                            Label = "England average",
                            Data = [ new Measure { Value = englishAndMathsResultsModel.EnglandNonDisadvantaged.CurrentYear, Unit = DataUnit.Percentage }]
                        },
                    ],
        };

        return new AcademicPerformanceEnglishAndMathsResultsViewModel
        {
            URN = englishAndMathsResultsModel.Urn,
            SchoolName = englishAndMathsResultsModel.SchoolName,
            IsKS2 = englishAndMathsResultsModel.IsKS2,
            IsKS4 = englishAndMathsResultsModel.IsKS4,
            IsKS5 = englishAndMathsResultsModel.IsKS5,
            SelectedGrade = selectedGrade,
            AllGcseData = allGcseData,
            AllGcseOverTimeData = allGcseOverTimeData,
            BreakdownGcseData = breakdownGcseData,
            BreakdownDisadvantaged = disadvantagedBreakdownData,
            BreakdownNonDisadvantaged = nonDisadvantagedBreakdownData,
            HasEstablishmentData = hasEstablishmentData.ToDisplayField(),
        };
    }
}
