using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformanceSubjectScaledScoresViewModel : BaseViewModel
{
    public required DataViewModel AllReadData { get; set; }

    public required DataOverTimeViewModel AllReadOverTimeData { get; set; }

    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public static AcademicPerformanceSubjectScaledScoresViewModel Map(EstablishmentServiceModel establishment, KS2ScaledScoreModel scaledScoreModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(establishment.LAName);

        var hasEstablishmentData = new[]
        {
            scaledScoreModel.ReadAverageEstablishment.CurrentYear.Value,
            scaledScoreModel.ReadAverageEstablishment.PreviousYear.Value,
            scaledScoreModel.ReadAverageEstablishment.TwoYearsAgo.Value,
        }.All(d => d is double v && v != 0);

        var allReadData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                scaledScoreModel.ReadAverageEstablishment.CurrentYear.Value,
                scaledScoreModel.ReadAverageLA.CurrentYear.Value,
                scaledScoreModel.ReadAverageEngland.CurrentYear.Value
            ],
        };

        var allReadOverTimeData = new DataOverTimeViewModel
        {
            Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
            Datasets =
               [
                   new DatasetViewModel
                    {
                        Label = "School",
                        Data = [scaledScoreModel.ReadAverageEstablishment.TwoYearsAgo.Value, scaledScoreModel.ReadAverageEstablishment.PreviousYear.Value, scaledScoreModel.ReadAverageEstablishment.CurrentYear.Value],
                    },
                    new DatasetViewModel
                    {
                        Label = laAverageLabel,
                        Data = [scaledScoreModel.ReadAverageLA.TwoYearsAgo.Value, scaledScoreModel.ReadAverageLA.PreviousYear.Value, scaledScoreModel.ReadAverageLA.CurrentYear.Value],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [scaledScoreModel.ReadAverageEngland.TwoYearsAgo.Value, scaledScoreModel.ReadAverageEngland.PreviousYear.Value, scaledScoreModel.ReadAverageEngland.CurrentYear.Value],
                    }
               ],
        };



        return new AcademicPerformanceSubjectScaledScoresViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            AllReadData = allReadData,
            AllReadOverTimeData = allReadOverTimeData,
            HasEstablishmentData = hasEstablishmentData.ToDisplayField()
        };
    }
}