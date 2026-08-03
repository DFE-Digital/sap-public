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

    public static AcademicPerformanceSubjectScaledScoresViewModel Map(KS2ScaledScoreModel scaledScoreModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(scaledScoreModel.LAName);

        var hasEstablishmentData = new[]
        {
            scaledScoreModel.Read_Average_Establishment.CurrentYear.Value,
            scaledScoreModel.Read_Average_Establishment.PreviousYear.Value,
            scaledScoreModel.Read_Average_Establishment.TwoYearsAgo.Value,
        }.All(d => d is double v && v != 0);

        var allReadData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data =
            [
                scaledScoreModel.Read_Average_Establishment.CurrentYear.Value,
                scaledScoreModel.Read_Average_LA.CurrentYear.Value,
                scaledScoreModel.Read_Average_England.CurrentYear.Value
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
                        Data = [scaledScoreModel.Read_Average_Establishment.TwoYearsAgo.Value, scaledScoreModel.Read_Average_Establishment.PreviousYear.Value, scaledScoreModel.Read_Average_Establishment.CurrentYear.Value],
                    },
                    new DatasetViewModel
                    {
                        Label = laAverageLabel,
                        Data = [scaledScoreModel.Read_Average_LA.TwoYearsAgo.Value, scaledScoreModel.Read_Average_LA.PreviousYear.Value, scaledScoreModel.Read_Average_LA.CurrentYear.Value],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [scaledScoreModel.Read_Average_England.TwoYearsAgo.Value, scaledScoreModel.Read_Average_England.PreviousYear.Value, scaledScoreModel.Read_Average_England.CurrentYear.Value],
                    }
               ],
        };



        return new AcademicPerformanceSubjectScaledScoresViewModel
        {
            URN = scaledScoreModel.Urn,
            SchoolName = scaledScoreModel.SchoolName,
            IsKS2 = scaledScoreModel.IsKS2,
            IsKS4 = scaledScoreModel.IsKS4,
            IsKS5 = scaledScoreModel.IsKS5,
            AllReadData = allReadData,
            AllReadOverTimeData = allReadOverTimeData,
            HasEstablishmentData = hasEstablishmentData.ToDisplayField()
        };
    }
}