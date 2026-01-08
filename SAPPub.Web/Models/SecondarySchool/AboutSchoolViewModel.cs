using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.SecondarySchool
{
    public class AboutSchoolViewModel : SecondarySchoolBaseViewModel
    {
        public record School(string Name, double Lat, double Lon);

        public string? AcademyTrust { get; set; }

        public string? AcademyTrustUpdatedIn { get; set; }

        public string? Website { get; set; }

        public string? Telephone { get; set; }

        public string? Address { get; set; }

        public string? LocalAuthority { get; set; }

        public string? LocalAuthorityWebsite { get; set; }

        public string? YourDistanceFromThisSchool { get; set; }

        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;    

        public static AboutSchoolViewModel Map(Establishment establishment)
        {
            var longLat = Helpers.MappingHelper.ConvertToLongLat(establishment.Easting, establishment.Northing);

            return new AboutSchoolViewModel
            {
                URN = establishment.URN,
                SchoolName = establishment.EstablishmentName,
                AcademyTrust = establishment.TrustName,
                Website = establishment.Website,
                Telephone = establishment.TelephoneNum,
                Address = establishment.Address,
                LocalAuthority = establishment.LANAme,
                LocalAuthorityWebsite = "https://www.gov.uk", // ToDo - Lookup from list
                YourDistanceFromThisSchool = "500m", // ToDo - calculate from input location,
                Latitude = longLat?.Latitude.ToString() ?? string.Empty,
                Longitude = longLat?.Longitude.ToString() ?? string.Empty
            };
        }
    }
}
