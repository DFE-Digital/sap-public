using SAPPub.Core.ServiceModels.Destinations;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Destinations;

public class KS4DestinationsViewModel : ProfileBaseViewModel
{
    public required DataViewModel AllDestinationsData { get; set; }
    public required DataViewModel AllDestinationsDisadvantagedData { get; set; }
    public required DataViewModel AllDestinationsNonDisadvantagedData { get; set; }

    public required DataOverTimeViewModel AllDestinationsOverTimeData { get; set; }

    public required SeriesViewModel BreakdownDestinationData { get; set; }

    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public static KS4DestinationsViewModel Map(KS4DestinationsDetails destinationsDetails)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(destinationsDetails.LocalAuthorityName);

        var hasEstablishmentData = new[]
        {
            destinationsDetails.SchoolAll.CurrentYear.Value,
            destinationsDetails.SchoolAll.PreviousYear.Value,
            destinationsDetails.SchoolAll.TwoYearsAgo.Value,
        }.All(d => d is double v && v != 0);

        return new KS4DestinationsViewModel
        {
            URN = destinationsDetails.Urn,
            SchoolName = destinationsDetails.SchoolName,
            IsKS2 = destinationsDetails.IsKS2,
            IsKS4 = destinationsDetails.IsKS4,
            IsKS5 = destinationsDetails.IsKS5,
            AllDestinationsData = new DataViewModel    {
                Labels = ["School", laAverageLabel, "England average"],
                Data = [destinationsDetails.SchoolAll.CurrentYear.Value, destinationsDetails.LocalAuthorityAll.CurrentYear.Value, destinationsDetails.EnglandAll.CurrentYear.Value],
            },
            AllDestinationsDisadvantagedData = new DataViewModel
            {
                Labels = ["School", laAverageLabel, "England average"],
                Data = [destinationsDetails.SchoolDisadvantagedAll.CurrentYear.Value, destinationsDetails.LocalAuthorityDisadvantagedAll.CurrentYear.Value, destinationsDetails.EnglandDisadvantagedAll.CurrentYear.Value],
            },
            AllDestinationsNonDisadvantagedData = new DataViewModel
            {
                Labels = [laAverageLabel, "England average"],
                Data = [destinationsDetails.LocalAuthorityNonDisadvantagedAll.CurrentYear.Value, destinationsDetails.EnglandNonDisadvantagedAll.CurrentYear.Value],

            },
            AllDestinationsOverTimeData = new DataOverTimeViewModel
            {
                Labels = ["2020 to 2021", "2021 to 2022", "2022 to 2023"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
                Datasets =
                [
                    new DatasetViewModel
                    {
                        Label = "School",
                        Data = [destinationsDetails.SchoolAll.TwoYearsAgo.Value, destinationsDetails.SchoolAll.PreviousYear.Value, destinationsDetails.SchoolAll.CurrentYear.Value],
                    },
                    new DatasetViewModel
                    {
                        Label = laAverageLabel,
                        Data = [destinationsDetails.LocalAuthorityAll.TwoYearsAgo.Value, destinationsDetails.LocalAuthorityAll.PreviousYear.Value, destinationsDetails.LocalAuthorityAll.CurrentYear.Value],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [destinationsDetails.EnglandAll.TwoYearsAgo.Value, destinationsDetails.EnglandAll.PreviousYear.Value, destinationsDetails.EnglandAll.CurrentYear.Value],
                    }
                ],               
            },
            BreakdownDestinationData = new SeriesViewModel {
                Labels = ["Staying in education", "Entering employment and apprenticeships"],
                Datasets =
                [
                    new DataSeriesViewModel {
                        Label = "School",
                        Data = [destinationsDetails.SchoolEducation.CurrentYear.Value, CommonHelper.AddNullable(destinationsDetails.SchoolEmployment.CurrentYear.Value, destinationsDetails.SchoolApprentice.CurrentYear.Value)]
                    },
                    new DataSeriesViewModel {
                        Label = laAverageLabel,
                        Data = [destinationsDetails.LocalAuthorityEducation.CurrentYear.Value, CommonHelper.AddNullable(destinationsDetails.LocalAuthorityEmployment.CurrentYear.Value, destinationsDetails.LocalAuthorityApprentice.CurrentYear.Value)]
                    },
                    new DataSeriesViewModel {
                        Label = "England average",
                        Data = [destinationsDetails.EnglandEducation.CurrentYear.Value, CommonHelper.AddNullable(destinationsDetails.EnglandEmployment.CurrentYear.Value, destinationsDetails.EnglandApprentice.CurrentYear.Value)]
                    },
                ],
            },
            HasEstablishmentData = hasEstablishmentData.ToDisplayField(),
        };
    }    
}
