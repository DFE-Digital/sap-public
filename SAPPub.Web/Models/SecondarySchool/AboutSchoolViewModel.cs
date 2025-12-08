namespace SAPPub.Web.Models.SecondarySchool
{
    public class AboutSchoolViewModel : SecondarySchoolBaseViewModel
    {
        public record School(string Name, double Lat, double Lon);

        public required string Name { get; set; }

        public string? AcademyTrust { get; set; }

        public string? AcademyTrustUpdatedIn { get; set; }

        public string? Website { get; set; }

        public required string Telephone { get; set; }

        public required string Address { get; set; }

        public string? LocalAuthority { get; set; }

        public string? LocalAuthorityWebsite { get; set; }

        public string? YouDistanceFromThisSchool { get; set; }
    }
}
