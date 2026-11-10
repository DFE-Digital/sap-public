using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class RetainedAndAssessedViewModel
{
    public required DisplayField<CodedDouble> NumberOfStudents { get; init; }

    public required DataViewModel AllData { get; set; }

    public required DataOverTimeViewModel AllOverTimeData { get; set; }

    public static RetainedAndAssessedViewModel Map(RetainedAndAssessedModel model, string laName)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(laName);

        var allData = new DataViewModel
        {
            Labels = [SchoolOrCollege, laAverageLabel, EnglandAverage],
            Data =
            [
                model.EstablishmentAll.CurrentYear.Value,
                model.LocalAuthorityAll.CurrentYear.Value,
                model.EnglandAll.CurrentYear.Value
            ],
        };

        var allDataOverTimeData = new DataOverTimeViewModel
        {
            //Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
            Labels = 
            [
                AcademicYearSelection.Previous2.GetDisplayName()!,
                AcademicYearSelection.Previous.GetDisplayName()!,
                AcademicYearSelection.Current.GetDisplayName()!
            ],
            Datasets =
            [
                new DatasetViewModel
                {
                    Label = SchoolOrCollege,
                    Data = [model.EstablishmentAll.TwoYearsAgo.Value, model.EstablishmentAll.PreviousYear.Value, model.EstablishmentAll.CurrentYear.Value],
                },
                new DatasetViewModel
                {
                    Label = laAverageLabel,
                    Data = [model.LocalAuthorityAll.TwoYearsAgo.Value, model.LocalAuthorityAll.PreviousYear.Value, model.LocalAuthorityAll.CurrentYear.Value],
                },
                new DatasetViewModel
                {
                    Label = EnglandAverage,
                    Data = [model.EnglandAll.TwoYearsAgo.Value, model.EnglandAll.PreviousYear.Value, model.EnglandAll.CurrentYear.Value],
                }
            ],
        };

        return new RetainedAndAssessedViewModel
        {
            NumberOfStudents = model.NumberOfStudents.CurrentYear.ToDisplayField(),
            AllData = allData,
            AllOverTimeData = allDataOverTimeData
        };
    }
}
