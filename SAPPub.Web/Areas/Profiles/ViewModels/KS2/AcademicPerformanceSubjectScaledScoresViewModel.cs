using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformanceSubjectScaledScoresViewModel : BaseViewModel
{
    public required DataViewModel AllReadData { get; set; }

    public required DataOverTimeViewModel AllReadOverTimeData { get; set; }

    public required DataViewModel AllMathsData { get; set; }

    public required DataOverTimeViewModel AllMathsOverTimeData { get; set; }

    public required DisplayField<bool> HasReadEstablishmentData { get; set; }

    public required DisplayField<bool> HasMathsEstablishmentData { get; set; }


    public static AcademicPerformanceSubjectScaledScoresViewModel Map(EstablishmentServiceModel establishment, KS2ScaledScoreModel scaledScoreModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(establishment.LAName);

        var hasReadEstablishmentData = new[]
        {
            scaledScoreModel.ReadAverageEstablishment.CurrentYear.Value,
            scaledScoreModel.ReadAverageEstablishment.PreviousYear.Value,
            scaledScoreModel.ReadAverageEstablishment.TwoYearsAgo.Value,
        }.All(d => d is double v && v != 0);

        var hasMathsEstablishmentData = new[]
{
            scaledScoreModel.MathsAverageEstablishment.CurrentYear.Value,
            scaledScoreModel.MathsAverageEstablishment.PreviousYear.Value,
            scaledScoreModel.MathsAverageEstablishment.TwoYearsAgo.Value,
        }.All(d => d is double v && v != 0);


        var allReadData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data = [ scaledScoreModel.ReadAverageEstablishment.CurrentYear.Value, scaledScoreModel.ReadAverageLA.CurrentYear.Value, scaledScoreModel.ReadAverageEngland.CurrentYear.Value ],
        };

        var allMathsData = new DataViewModel
        {
            Labels = ["School", laAverageLabel, "England average"],
            Data = [scaledScoreModel.MathsAverageEstablishment.CurrentYear.Value, scaledScoreModel.MathsAverageLA.CurrentYear.Value, scaledScoreModel.MathsAverageEngland.CurrentYear.Value],
        };

        var allReadOverTimeData = GetDataOverTimeViewModel(
            scaledScoreModel.ReadAverageEstablishment.TwoYearsAgo.Value, scaledScoreModel.ReadAverageEstablishment.PreviousYear.Value, scaledScoreModel.ReadAverageEstablishment.CurrentYear.Value,
            scaledScoreModel.ReadAverageLA.TwoYearsAgo.Value, scaledScoreModel.ReadAverageLA.PreviousYear.Value, scaledScoreModel.ReadAverageLA.CurrentYear.Value,
            scaledScoreModel.ReadAverageEngland.TwoYearsAgo.Value, scaledScoreModel.ReadAverageEngland.PreviousYear.Value, scaledScoreModel.ReadAverageEngland.CurrentYear.Value,
            laAverageLabel);

        var allMathsOverTimeData = GetDataOverTimeViewModel(
            scaledScoreModel.MathsAverageEstablishment.TwoYearsAgo.Value, scaledScoreModel.MathsAverageEstablishment.PreviousYear.Value, scaledScoreModel.MathsAverageEstablishment.CurrentYear.Value,
            scaledScoreModel.MathsAverageLA.TwoYearsAgo.Value, scaledScoreModel.MathsAverageLA.PreviousYear.Value, scaledScoreModel.MathsAverageLA.CurrentYear.Value,
            scaledScoreModel.MathsAverageEngland.TwoYearsAgo.Value, scaledScoreModel.MathsAverageEngland.PreviousYear.Value, scaledScoreModel.MathsAverageEngland.CurrentYear.Value,
            laAverageLabel);

        return new AcademicPerformanceSubjectScaledScoresViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            AllReadData = allReadData,
            AllReadOverTimeData = allReadOverTimeData,
            AllMathsData = allMathsData,
            AllMathsOverTimeData = allMathsOverTimeData,
            HasReadEstablishmentData = hasReadEstablishmentData.ToDisplayField(),
            HasMathsEstablishmentData = hasMathsEstablishmentData.ToDisplayField()
        };
    }
}