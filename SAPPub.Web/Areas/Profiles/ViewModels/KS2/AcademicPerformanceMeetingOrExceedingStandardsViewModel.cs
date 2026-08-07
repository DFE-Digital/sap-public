using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformanceMeetingOrExceedingStandardsViewModel : BaseViewModel
{
    public required DataViewModel AllReadData { get; set; }

    public required DataOverTimeViewModel AllReadOverTimeData { get; set; }

    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public static AcademicPerformanceMeetingOrExceedingStandardsViewModel Map(
        EstablishmentServiceModel establishment,
        KS2MeetingOrExceedingStandardsModel kS2MeetingOrExceedingStandardsModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(establishment.LAName);


        var hasEstablishmentData = new[]
        {
            kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.CurrentYear.Value,
            kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.PreviousYear.Value,
            kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.TwoYearsAgo.Value,
        }.All(d => d is double v && v != 0);


        var allPercentageData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.CurrentYear.Value,
                kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentage.CurrentYear.Value,
                kS2MeetingOrExceedingStandardsModel.EnglandPercentage.CurrentYear.Value
            ],
        };

        var allPercentageDataOverTimeData = new DataOverTimeViewModel
        {
            Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
            Datasets =
            [
                new DatasetViewModel
                {
                    Label = "School",
                    Data = [kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.EstablishmentPercentage.CurrentYear.Value],
                },
                new DatasetViewModel
                {
                    Label = laAverageLabel,
                    Data = [kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentage.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentage.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentage.CurrentYear.Value],
                },
                new DatasetViewModel
                {
                    Label = "England average",
                    Data = [kS2MeetingOrExceedingStandardsModel.EnglandPercentage.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.EnglandPercentage.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.EnglandPercentage.CurrentYear.Value],
                }
            ],
        };

        return new AcademicPerformanceMeetingOrExceedingStandardsViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            AllReadData = allPercentageData,
            AllReadOverTimeData = allPercentageDataOverTimeData,
            HasEstablishmentData = hasEstablishmentData.ToDisplayField()

        };
    }
}