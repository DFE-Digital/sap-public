using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareDestinationsViewModel : CompareSecondarySchoolBaseViewModel
{
    public class SchoolDestinationDetails
    {
        public required string URN { get; set; }
        public required string SchoolName { get; set; }
        public required DisplayField<bool> SixthForm { get; set; }
        public required double? PercentInEducationEmploymentOrTraining { get; set; }

        public static SchoolDestinationDetails Map(SAPPub.Core.ServiceModels.Compare.SchoolDestinationDetails destinationsDetails, Establishment establishmentDetails)
        {
            return new SchoolDestinationDetails
            {
                URN = destinationsDetails.URN,
                SchoolName = establishmentDetails.EstablishmentName,
                SixthForm = GetSixthForm(establishmentDetails.OfficialSixthFormId).ToDisplayField(),
                PercentInEducationEmploymentOrTraining = destinationsDetails.PercentInEducationEmploymentOrTraining,
            };
        }

        private static bool? GetSixthForm(string value)
        {
            return string.Equals(value, "1");
        }
    }

    public required double? EnglandPercentage { get; set; }

    public required DataViewModel AllDestinationsData { get; set; }

    public required IEnumerable<SchoolDestinationDetails> SchoolDetails { get; set; }

    public static CompareDestinationsViewModel Map(List<string> urns, DestinationsComparisonResultModel destinationsDetails, List<Establishment> establishments)
    {
        var schoolDetails = destinationsDetails
                .SchoolDetails
                .Select(d => SchoolDestinationDetails.Map(d, establishments.First(e => e.URN == d.URN)))
                .OrderBy(d => d.SchoolName)
                .ToList();
        return new CompareDestinationsViewModel
        {
            URNs = urns,
            EnglandPercentage = destinationsDetails.EnglandPercentage,
            SchoolDetails = schoolDetails,
            AllDestinationsData = new DataViewModel // save a version of the data reshaped to send into the chart component
            {
                Labels = schoolDetails
                    .Select(d => d.SchoolName)
                    .Concat(["England average"])
                    .ToList(),
                Data = schoolDetails
                    .Select(d => d.PercentInEducationEmploymentOrTraining)
                    .Concat([destinationsDetails.EnglandPercentage])
                    .ToList()
            }
        };
    }
}
