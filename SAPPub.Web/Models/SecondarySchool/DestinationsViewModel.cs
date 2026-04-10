using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class DestinationsViewModel : SecondarySchoolBaseViewModel
{
    public required DataViewModel AllDestinationsData { get; set; }

    public required DataOverTimeViewModel AllDestinationsOverTimeData { get; set; }

    public required SeriesViewModel BreakdownDestinationData { get; set; }

    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public static DestinationsViewModel Map(DestinationsDetails destinationsDetails)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(destinationsDetails.LocalAuthorityName);

        var hasEstablishmentData = new[]
        {
            destinationsDetails.SchoolAll.CurrentYear,
            destinationsDetails.SchoolAll.PreviousYear,
            destinationsDetails.SchoolAll.TwoYearsAgo,
        }.All(d => d is double v && v != 0);

        return new DestinationsViewModel
        {
            URN = destinationsDetails.Urn,
            SchoolName = destinationsDetails.SchoolName,
            AllDestinationsData = new DataViewModel    {
                Labels = ["School", laAverageLabel, "England average"],
                Data = [destinationsDetails.SchoolAll.CurrentYear, destinationsDetails.LocalAuthorityAll.CurrentYear, destinationsDetails.EnglandAll.CurrentYear],
            },
            AllDestinationsOverTimeData = new DataOverTimeViewModel
            {
                Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
                Datasets =
                [
                    new DatasetViewModel
                    {
                        Label = "School",
                        Data = [destinationsDetails.SchoolAll.TwoYearsAgo, destinationsDetails.SchoolAll.PreviousYear, destinationsDetails.SchoolAll.CurrentYear],
                    },
                    new DatasetViewModel
                    {
                        Label = laAverageLabel,
                        Data = [destinationsDetails.LocalAuthorityAll.TwoYearsAgo, destinationsDetails.LocalAuthorityAll.PreviousYear, destinationsDetails.LocalAuthorityAll.CurrentYear],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [destinationsDetails.EnglandAll.TwoYearsAgo, destinationsDetails.EnglandAll.PreviousYear, destinationsDetails.EnglandAll.CurrentYear],
                    }
                ],               
            },
            BreakdownDestinationData = new SeriesViewModel {
                Labels = ["Staying in education", "Entering employment and apprenticeships"],
                Datasets =
                [
                    new DataSeriesViewModel {
                        Label = "School",
                        Data = [destinationsDetails.SchoolEducation.CurrentYear, CommonHelper.AddNullable(destinationsDetails.SchoolEmployment.CurrentYear, destinationsDetails.SchoolApprentice.CurrentYear)]
                    },
                    new DataSeriesViewModel {
                        Label = laAverageLabel,
                        Data = [destinationsDetails.LocalAuthorityEducation.CurrentYear, CommonHelper.AddNullable(destinationsDetails.LocalAuthorityEmployment.CurrentYear, destinationsDetails.LocalAuthorityApprentice.CurrentYear)]
                    },
                    new DataSeriesViewModel {
                        Label = "England average",
                        Data = [destinationsDetails.EnglandEducation.CurrentYear, CommonHelper.AddNullable(destinationsDetails.EnglandEmployment.CurrentYear, destinationsDetails.EnglandApprentice.CurrentYear)]
                    },
                ],
            },
            HasEstablishmentData = hasEstablishmentData.ToDisplayField(),
        };
    }    
}
