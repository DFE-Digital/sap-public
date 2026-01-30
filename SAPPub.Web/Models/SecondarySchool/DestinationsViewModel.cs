using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class DestinationsViewModel : SecondarySchoolBaseViewModel
{
    public required DataViewModel AllDestinationsData { get; set; }

    public required DataOverTimeViewModel AllDestinationsOverTimeData { get; set; }

    public static DestinationsViewModel Map(DestinationsDetails destinationsDetails)
    {
        return new DestinationsViewModel
        {
            URN = destinationsDetails.Urn,
            SchoolName = destinationsDetails.SchoolName,
            AllDestinationsData = new DataViewModel    {
                Labels = ["School", $"{destinationsDetails.LocalAuthorityName} average", "England average"],
                Data = [destinationsDetails.SchoolAll.CurrentYear ?? 0, destinationsDetails.LocalAuthorityAll.CurrentYear ?? 0, destinationsDetails.EnglandAll.CurrentYear ?? 0],
            },
            AllDestinationsOverTimeData = new DataOverTimeViewModel
            {
                Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
                Datasets =
                [
                    new DatasetViewModel
                    {
                        Label = "School",
                        Data = [destinationsDetails.SchoolAll.TwoYearsAgo ?? 0, destinationsDetails.SchoolAll.PreviousYear ?? 0, destinationsDetails.SchoolAll.CurrentYear ?? 0],
                    },
                    new DatasetViewModel
                    {
                        Label = $"{destinationsDetails.LocalAuthorityName} average",
                        Data = [destinationsDetails.LocalAuthorityAll.TwoYearsAgo ?? 0, destinationsDetails.LocalAuthorityAll.PreviousYear ?? 0, destinationsDetails.LocalAuthorityAll.CurrentYear ?? 0],
                    },
                    new DatasetViewModel
                    {
                        Label = "England average",
                        Data = [destinationsDetails.EnglandAll.TwoYearsAgo ?? 0, destinationsDetails.EnglandAll.PreviousYear ?? 0, destinationsDetails.EnglandAll.CurrentYear ?? 0],
                    }
                ],               
            },
        };
    }
}
