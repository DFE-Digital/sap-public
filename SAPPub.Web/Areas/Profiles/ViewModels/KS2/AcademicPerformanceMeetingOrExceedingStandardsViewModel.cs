using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Areas.Profiles.ViewModels.Performance;
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

    public required MeetingExceedingStandardsViewModel GirlsAndBoys { get; set; }
    public required MeetingExceedingStandardsViewModel EnglishAsAnAdditionalLanguage { get; set; }
    public required MeetingExceedingStandardsViewModel NonMobilePupils { get; set; }
    public required MeetingExceedingStandardsViewModel DisadvantagedPupils { get; set; }
    public required MeetingExceedingStandardsViewModel NonDisadvantagedPupils { get; set; }

    public static AcademicPerformanceMeetingOrExceedingStandardsViewModel Map(
        EstablishmentMinimumServiceModel establishment,
        KS2MeetingOrExceedingStandardsModel ks2MESModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(establishment.LAName);

        var allPercentageDataMeetingOrExceeding = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                ks2MESModel.EstablishmentPercentageMeetingOrExceeding.CurrentYear.Value,
                ks2MESModel.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear.Value,
                ks2MESModel.EnglandPercentageMeetingOrExceeding.CurrentYear.Value
            ],
        };

        var allPercentageDataExceeding = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                ks2MESModel.EstablishmentPercentageExceeding.CurrentYear.Value,
                ks2MESModel.LocalAuthorityPercentageExceeding.CurrentYear.Value,
                ks2MESModel.EnglandPercentageExceeding.CurrentYear.Value
            ],
        };

        var allPercentageDataOverTimeDataMeetingOrExceeding = GetDataOverTimeViewModel(
            ks2MESModel.EstablishmentPercentageMeetingOrExceeding.TwoYearsAgo.Value, ks2MESModel.EstablishmentPercentageMeetingOrExceeding.PreviousYear.Value, ks2MESModel.EstablishmentPercentageMeetingOrExceeding.CurrentYear.Value,
            ks2MESModel.LocalAuthorityPercentageMeetingOrExceeding.TwoYearsAgo.Value, ks2MESModel.LocalAuthorityPercentageMeetingOrExceeding.PreviousYear.Value, ks2MESModel.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear.Value,
            ks2MESModel.EnglandPercentageMeetingOrExceeding.TwoYearsAgo.Value, ks2MESModel.EnglandPercentageMeetingOrExceeding.PreviousYear.Value, ks2MESModel.EnglandPercentageMeetingOrExceeding.CurrentYear.Value,
            laAverageLabel);

        var allPercentageDataOverTimeDataExceeding = GetDataOverTimeViewModel(
            ks2MESModel.EstablishmentPercentageExceeding.TwoYearsAgo.Value, ks2MESModel.EstablishmentPercentageExceeding.PreviousYear.Value, ks2MESModel.EstablishmentPercentageExceeding.CurrentYear.Value,
            ks2MESModel.LocalAuthorityPercentageExceeding.TwoYearsAgo.Value, ks2MESModel.LocalAuthorityPercentageExceeding.PreviousYear.Value, ks2MESModel.LocalAuthorityPercentageExceeding.CurrentYear.Value,
            ks2MESModel.EnglandPercentageExceeding.TwoYearsAgo.Value, ks2MESModel.EnglandPercentageExceeding.PreviousYear.Value, ks2MESModel.EnglandPercentageExceeding.CurrentYear.Value,
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
            GirlsAndBoys = GetMeetingExceedingStandardsViewModel(PupilGroup,
                [
                    new() { RowTitle = "Girls", MeetingStandard = ks2MESModel.GirlsMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.GirlsExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = "Boys", MeetingStandard = ks2MESModel.BoysMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.BoysExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = AllPupilsAtTheSchool, MeetingStandard = ks2MESModel.AllPupilsMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.AllPupilsExceedingExpectedStandard.ToDisplayField() },
                ]),
            EnglishAsAnAdditionalLanguage = GetMeetingExceedingStandardsViewModel(PupilGroup,
                [
                    new() { RowTitle = "Pupils with EAL", MeetingStandard = ks2MESModel.EALMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.EALExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = AllPupilsAtTheSchool, MeetingStandard = ks2MESModel.AllPupilsMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.AllPupilsExceedingExpectedStandard.ToDisplayField() },
                ]),
            NonMobilePupils = GetMeetingExceedingStandardsViewModel(PupilGroup,
                [
                    new() { RowTitle = "Non-mobile pupils", MeetingStandard = ks2MESModel.NonMobileMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.NonMobileExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = AllPupilsAtTheSchool, MeetingStandard = ks2MESModel.AllPupilsMeetingExpectedStandard.ToDisplayField() , ExceedingStandard = ks2MESModel.AllPupilsExceedingExpectedStandard.ToDisplayField() },
                ]),
            DisadvantagedPupils = GetMeetingExceedingStandardsViewModel($"{PupilGroup} (Disadvantaged)",
                [
                    new() { RowTitle = "School", MeetingStandard = ks2MESModel.EstablishmentDisadvantagedMeetingExpectedStandard.ToDisplayField(), ExceedingStandard = ks2MESModel.EstablishmentDisadvantagedExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = $"{establishment.LAName} average", MeetingStandard = ks2MESModel.LocalAuthorityDisadvantagedMeetingExpectedStandard.ToDisplayField(), ExceedingStandard = ks2MESModel.LocalAuthorityDisadvantagedExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = Constants.Constants.EnglandAverage,  MeetingStandard = ks2MESModel.EnglandDisadvantagedMeetingExpectedStandard.ToDisplayField(), ExceedingStandard = ks2MESModel.EnglandDisadvantagedExceedingExpectedStandard.ToDisplayField() },

                ]),
            NonDisadvantagedPupils = GetMeetingExceedingStandardsViewModel($"{PupilGroup} (Non-disadvantaged)",
                [
                    new() { RowTitle = $"{establishment.LAName} average", MeetingStandard = ks2MESModel.LocalAuthorityNonDisadvantagedMeetingExpectedStandard.ToDisplayField(), ExceedingStandard = ks2MESModel.LocalAuthorityNonDisadvantagedExceedingExpectedStandard.ToDisplayField() },
                    new() { RowTitle = Constants.Constants.EnglandAverage, MeetingStandard = ks2MESModel.EnglandNonDisadvantagedMeetingExpectedStandard.ToDisplayField(), ExceedingStandard = ks2MESModel.EnglandNonDisadvantagedExceedingExpectedStandard.ToDisplayField() },
                ])
        };
    }


    private static MeetingExceedingStandardsViewModel GetMeetingExceedingStandardsViewModel(string column1Title, List<MeetingExceedingStandardsDetailViewModel> scaledScoresDetailViewModels)
    {
        return new MeetingExceedingStandardsViewModel
        {
            Column1Title = column1Title,
            Rows = scaledScoresDetailViewModels.Select(a =>
                    new MeetingExceedingStandardsDetailViewModel
                    {
                        RowTitle = a.RowTitle,
                        ExceedingStandard = a.ExceedingStandard,
                        MeetingStandard = a.MeetingStandard
                    })
        };
    }
}