using SAPPub.Core.Helpers;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Common;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareAboutYourSchoolsViewModel : CompareSecondarySchoolBaseViewModel
{
    public IEnumerable<CompareAboutYourSchoolViewModel> CompareAboutSchools { get; set; } = [];

    public IEnumerable<MapData> MapData { get; set; } = [];

    public class CompareAboutYourSchoolViewModel
    {
        public required string Urn { get; set; }

        public required string SchoolName { get; set; }

        public string? Website { get; set; }

        public DisplayField<string> Address { get; set; }

        public string? LocalAuthority { get; set; }

        public string? LocalAuthorityName { get; set; }

        public string? LocalAuthorityWebsite { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }


    public static CompareAboutYourSchoolsViewModel Map(
        List<string> urns,
        IEnumerable<AboutSchoolComparisonModel> aboutSchoolComparisonModelList)
    {
        var compareAboutYourSchoolsViewModel = new CompareAboutYourSchoolsViewModel
        {
            URNs = urns,
            CompareAboutSchools = aboutSchoolComparisonModelList.Select(r =>
            {
                var latLng = MappingHelper.ConvertToLatLon(r.Easting, r.Northing);

                return new CompareAboutYourSchoolViewModel
                {
                    Urn = r.Urn,
                    SchoolName = r.SchoolName,
                    Website = r.Website,
                    Address = r.Address.ToDisplayField(),
                    LocalAuthority = r.LocalAuthority,
                    LocalAuthorityName = r.LocalAuthorityName,
                    LocalAuthorityWebsite = r.LocalAuthorityWebsite,
                    Latitude = latLng?.Latitude,
                    Longitude = latLng?.Longitude
                };
            })
        };

        compareAboutYourSchoolsViewModel.MapData =
                compareAboutYourSchoolsViewModel.CompareAboutSchools
                .Where(r => r.Latitude is not null || r.Longitude is not null)
                .Select(r => new MapData { Lat = r.Latitude, Lng = r.Longitude, Name = r.SchoolName });

        return compareAboutYourSchoolsViewModel;
    }
}