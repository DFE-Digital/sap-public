using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Web.Models.Charts;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareDestinationsViewModel : CompareSecondarySchoolBaseViewModel
{
    public class SchoolDestinationDetailsViewModel
    {
        public required string URN { get; set; }
        public required string SchoolName { get; set; }
        public required bool? SixthForm { get; set; }
        public required double? PercentInEducationEmploymentOrTraining { get; set; }

        public static SchoolDestinationDetailsViewModel Map(SAPPub.Core.ServiceModels.Compare.SchoolDestinationDetails destinationsDetails, Establishment establishmentDetails)
        {
            return new SchoolDestinationDetailsViewModel
            {
                URN = destinationsDetails.URN,
                SchoolName = establishmentDetails.EstablishmentName,
                SixthForm = GetSixthForm(establishmentDetails.OfficialSixthFormId),
                PercentInEducationEmploymentOrTraining = destinationsDetails.PercentInEducationEmploymentOrTraining,
            };
        }

        private static bool? GetSixthForm(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "9" || value == "0")
            {
                return null;
            }
            return string.Equals(value, "1");
        }
    }

    public required double? EnglandPercentage { get; set; }

    public required DataViewModel AllDestinationsData { get; set; }

    public required IEnumerable<SchoolDestinationDetailsViewModel> SchoolDetails { get; set; }

    public static CompareDestinationsViewModel Map(List<string> urns, DestinationsComparisonResultModel destinationsDetails, List<Establishment> establishments)
    {
        var schoolDetails = destinationsDetails.SchoolDetails
            .Join(
                establishments,
                d => d.URN,
                e => e.URN,
                (d, e) => SchoolDestinationDetailsViewModel.Map(d, e))
            .OrderBy(d => d.SchoolName)
            .ToList();

        return new CompareDestinationsViewModel
        {
            URNs = urns,
            EnglandPercentage = destinationsDetails.EnglandPercentage,
            SchoolDetails = schoolDetails,
            AllDestinationsData = new DataViewModel // save a version of the data reshaped to send into the chart component
            {
                Labels = [.. schoolDetails.Select(d => d.SchoolName), "England average"],
                Data = [.. schoolDetails.Select(d => d.PercentInEducationEmploymentOrTraining), destinationsDetails.EnglandPercentage],
                BackgroundColors = [.. Enumerable.Repeat(EstablishmentChartColour, schoolDetails.Count), EnglandAverageChartColour]
            }
        };
    }
}
