using SAPPub.Core.Entities;
using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;

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
                LocalAuthority = establishment.LAName,
                LocalAuthorityWebsite = "https://www.gov.uk", // ToDo - Lookup from list
                YourDistanceFromThisSchool = "500m", // ToDo - calculate from input location,
                Latitude = longLat?.Latitude.ToString() ?? string.Empty,
                Longitude = longLat?.Longitude.ToString() ?? string.Empty,
                TypeOfSchool = establishment.TypeOfEstablishmentName,
                HeadTeacher = establishment.Headteacher,
                AgeRange = establishment.AgeRange,
                NumberOfPupils = establishment.TotalPupils?.ToInt()?.ToString("N0") ?? establishment.TotalPupils,
                PupilSex = establishment.GenderName,
                ReligiousCharacter = establishment.ReligiousCharacterName,
                SixthForm = GetSixthForm(establishment.OfficialSixthFormId),
                SenUnit = GetSenUnit(establishment.ResourcedProvision),
                ResourcedProvision = GetResourcedProvision(establishment.ResourcedProvision),
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
