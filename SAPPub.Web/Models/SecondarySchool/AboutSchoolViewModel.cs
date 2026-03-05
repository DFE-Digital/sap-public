using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;

namespace SAPPub.Web.Models.SecondarySchool
{
    public class AboutSchoolViewModel : SecondarySchoolBaseViewModel
    {
        private const string SENUnit = "SEN unit";
        private const string ResourcedProvisionAndSENUnit = "Resourced provision and SEN unit";
        private const string ResourcedProvisionText = "Resourced provision";

        public record School(string Name, double Lat, double Lon);

        public string? AcademyTrust { get; set; }

        public string? AcademyTrustUpdatedIn { get; set; }

        public string? Website { get; set; }

        public string? Telephone { get; set; }

        public string? Address { get; set; }

        public string? LocalAuthority { get; set; }

        public string? LocalAuthorityCouncilName { get; set; }

        public string? LocalAuthorityWebsite { get; set; }

        public string? YourDistanceFromThisSchool { get; set; }

        public string Longitude { get; set; } = string.Empty;

        public string Latitude { get; set; } = string.Empty;

        public string? TypeOfSchool { get; set; }

        public string? HeadTeacher { get; set; }

        public string? AgeRange { get; set; }

        public string? NumberOfPupils { get; set; }

        public string? PupilSex { get; set; }

        public string? ReligiousCharacter { get; set; }

        public string? SixthForm { get; set; }

        public string? SenUnit { get; set; }

        public string? ResourcedProvision { get; set; }

        public static AboutSchoolViewModel Map(AboutSchoolModel schoolDetails)
        {
            var latLong = MappingHelper.ConvertToLatLon(schoolDetails.Easting, schoolDetails.Northing);

            return new AboutSchoolViewModel
            {
                URN = schoolDetails.Urn,
                SchoolName = schoolDetails.SchoolName,
                AcademyTrust = schoolDetails.AcademyTrust,
                Website = schoolDetails.Website,
                Telephone = schoolDetails.Telephone,
                Address = schoolDetails.Address,
                LocalAuthority = schoolDetails.LocalAuthority,
                LocalAuthorityCouncilName = schoolDetails.LocalAuthorityName,
                LocalAuthorityWebsite = schoolDetails.LocalAuthorityWebsite,
                Latitude = latLong?.Latitude.ToString() ?? string.Empty,
                Longitude = latLong?.Longitude.ToString() ?? string.Empty,
                TypeOfSchool = schoolDetails.TypeOfSchool,
                HeadTeacher = schoolDetails.HeadTeacher,
                AgeRange = schoolDetails.AgeRange,
                NumberOfPupils = schoolDetails.NumberOfPupils?.ToInt()?.ToString("N0") ?? schoolDetails.NumberOfPupils,
                PupilSex = schoolDetails.PupilSex,
                ReligiousCharacter = schoolDetails.ReligiousCharacter,
                SixthForm = GetSixthForm(schoolDetails.OfficialSixthFormId),
                SenUnit = GetSenUnit(schoolDetails.ResourcedProvision),
                ResourcedProvision = GetResourcedProvision(schoolDetails.ResourcedProvision),
            };
        }

        private static string GetSenUnit(string value)
        {
            return string.Equals(value, SENUnit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, ResourcedProvisionAndSENUnit, StringComparison.OrdinalIgnoreCase)
                ? Constants.Yes
                : Constants.No;
        }

        private static string GetResourcedProvision(string value)
        {
            return string.Equals(value, ResourcedProvisionText, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, ResourcedProvisionAndSENUnit, StringComparison.OrdinalIgnoreCase)
                ? Constants.Yes
                : Constants.No;
        }

        private static string GetSixthForm(string value)
        {
            return string.Equals(value, "1") ? Constants.Yes : Constants.No;
        }
    }
}
