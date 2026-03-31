using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;



namespace SAPPub.Web.Models.SecondarySchool
{
    public class AboutSchoolViewModel : SecondarySchoolBaseViewModel
    {
        private const string SENUnit = "SEN unit";
        private const string ResourcedProvisionAndSENUnit = "Resourced provision and SEN unit";
        private const string ResourcedProvisionText = "Resourced provision";
        private const int LocalAuthorityEstablishmentGroupTypeId = 4;

        public record School(string Name, double Lat, double Lon);

        public required DisplayField<string> AcademyTrust { get; set; }

        public required DisplayField<string> AcademyTrustUpdatedIn { get; set; }

        public required DisplayField<string> SchoolWebsite { get; set; }

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

        public bool IsLocalAuthoritySchool { get; set; }

        public required DisplayField<DateOnly> ClosedDate { get; set; }

        public int? StatusCode { get; set; }

        public bool IsSchoolClosed => StatusCode == 2;

        public static AboutSchoolViewModel Map(AboutSchoolModel schoolDetails)
        {
            var latLong = MappingHelper.ConvertToLatLon(schoolDetails.Easting, schoolDetails.Northing);

            return new AboutSchoolViewModel
            {
                URN = schoolDetails.Urn,
                SchoolName = schoolDetails.SchoolName,
                AcademyTrust = schoolDetails.AcademyTrust.ToDisplayField(),
                AcademyTrustUpdatedIn = schoolDetails.AcademyTrustUpdatedIn.ToDisplayField(),
                SchoolWebsite = schoolDetails.Website.ToDisplayField(),
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
                SenUnit = GetSenUnit(schoolDetails.ResourcedProvisionName),
                ResourcedProvision = GetResourcedProvision(schoolDetails.ResourcedProvisionName),
                IsLocalAuthoritySchool = schoolDetails.EstablishmentTypeGroupId.ToInt() == LocalAuthorityEstablishmentGroupTypeId,
                StatusCode = schoolDetails.StatusCode,
                ClosedDate = schoolDetails.ClosedDate.ToDisplayField(),
            };
        }

        private static string GetSenUnit(string value)
        {
            return string.Equals(value, SENUnit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, ResourcedProvisionAndSENUnit, StringComparison.OrdinalIgnoreCase)
                ? Yes
                : No;
        }

        private static string GetResourcedProvision(string value)
        {
            return string.Equals(value, ResourcedProvisionText, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, ResourcedProvisionAndSENUnit, StringComparison.OrdinalIgnoreCase)
                ? Yes
                : No;
        }

        private static string GetSixthForm(string value)
        {
            return string.Equals(value, "1") ? Yes : No;
        }
    }
}
