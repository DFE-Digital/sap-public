using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformanceMeetingOrExceedingStandardsViewModel : BaseViewModel
{
    public required DataViewModel AllMeetingExceedingStandardsData { get; set; }

    public required DataOverTimeViewModel AllMeetingExceedingStandardsOverTimeData { get; set; }

    public required DataViewModel AllExceedingStandardsData { get; set; }

    public required DataOverTimeViewModel AllExceedingStandardsOverTimeData { get; set; }

    public static AcademicPerformanceMeetingOrExceedingStandardsViewModel Map(
        EstablishmentServiceModel establishment,
        KS2MeetingOrExceedingStandardsModel kS2MeetingOrExceedingStandardsModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(establishment.LAName);

        var allPercentageDataMeetingOrExceeding = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageMeetingOrExceeding.CurrentYear.Value,
                kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear.Value,
                kS2MeetingOrExceedingStandardsModel.EnglandPercentageMeetingOrExceeding.CurrentYear.Value
            ],
        };

        var allPercentageDataExceeding = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageExceeding.CurrentYear.Value,
                kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageExceeding.CurrentYear.Value,
                kS2MeetingOrExceedingStandardsModel.EnglandPercentageExceeding.CurrentYear.Value
            ],
        };

        var allPercentageDataOverTimeDataMeetingOrExceeding = GetDataOverTimeViewModel(
            kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageMeetingOrExceeding.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageMeetingOrExceeding.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageMeetingOrExceeding.CurrentYear.Value,
            kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageMeetingOrExceeding.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageMeetingOrExceeding.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear.Value,
            kS2MeetingOrExceedingStandardsModel.EnglandPercentageMeetingOrExceeding.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.EnglandPercentageMeetingOrExceeding.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.EnglandPercentageMeetingOrExceeding.CurrentYear.Value,
            laAverageLabel);

        var allPercentageDataOverTimeDataExceeding = GetDataOverTimeViewModel(
            kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageExceeding.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageExceeding.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.EstablishmentPercentageExceeding.CurrentYear.Value,
            kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageExceeding.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageExceeding.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.LocalAuthorityPercentageExceeding.CurrentYear.Value,
            kS2MeetingOrExceedingStandardsModel.EnglandPercentageExceeding.TwoYearsAgo.Value, kS2MeetingOrExceedingStandardsModel.EnglandPercentageExceeding.PreviousYear.Value, kS2MeetingOrExceedingStandardsModel.EnglandPercentageExceeding.CurrentYear.Value,
            laAverageLabel);

        return new AcademicPerformanceMeetingOrExceedingStandardsViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            AllMeetingExceedingStandardsData = allPercentageDataMeetingOrExceeding,
            AllMeetingExceedingStandardsOverTimeData = allPercentageDataOverTimeDataMeetingOrExceeding,
            AllExceedingStandardsData = allPercentageDataExceeding,
            AllExceedingStandardsOverTimeData = allPercentageDataOverTimeDataExceeding,
        };
    }
}