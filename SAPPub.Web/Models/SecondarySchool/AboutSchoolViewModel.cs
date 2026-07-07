using SAPPub.Core.Enums;
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
        private const int AcademyOpenReasonId = 10;

        public record School(string Name, double Lat, double Lon);

        public required DisplayField<string> AcademyTrust { get; set; }

        public required DisplayField<string> AcademyTrustUpdatedIn { get; set; }

        public required DisplayField<string> SchoolWebsite { get; set; }

        public required DisplayField<string> Telephone { get; set; }

        public required DisplayField<string> Address { get; set; }

        public required DisplayField<string> LocalAuthority { get; set; }

        public string? LocalAuthorityCouncilName { get; set; }

        public string? LocalAuthorityWebsite { get; set; }

        public string? YourDistanceFromThisSchool { get; set; }

        public string Longitude { get; set; } = string.Empty;

        public string Latitude { get; set; } = string.Empty;

        public required DisplayField<string> TypeOfSchool { get; set; }

        public required DisplayField<string> HeadTeacher { get; set; }

        public required DisplayField<string> AgeRange { get; set; }

        public required DisplayField<string> NumberOfPupils { get; set; }

        public required DisplayField<string> PupilSex { get; set; }

        public required DisplayField<string> ReligiousCharacter { get; set; }

        public required DisplayField<string> SixthForm { get; set; }

        public string? SenUnit { get; set; }

        public string? ResourcedProvision { get; set; }

        public bool IsLocalAuthoritySchool { get; set; }

        public required DisplayField<DateOnly> ClosedDate { get; set; }
        public DateOnly? OpenDate { get; set; }

        public EstablishmentStatus? StatusCode { get; set; }

        public bool IsSchoolClosed => StatusCode == EstablishmentStatus.Closed;
        public int? OpenReasonId { get; set; }

        public required DisplayField<string> RecentlyOpenedSchoolMessage { get; set; }

        public bool HasPredecessors => Predecessors != null && Predecessors.Count > 0;
        public bool HasSuccessors => Successors != null;

        public required DisplayField<string> SenTypes { get; set; }
        public List<SuccessorOrPredecessorDetailsModel>? Predecessors { get; set; }
        public List<SuccessorOrPredecessorDetailsModel>? Successors { get; set; }

        public string? EducationPhase { get; set;  }
        public bool IsKS4 { get; set; }

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
                Telephone = schoolDetails.Telephone.ToDisplayField(),
                Address = schoolDetails.Address.ToDisplayField(),
                LocalAuthority = schoolDetails.LocalAuthority.ToDisplayField(),
                LocalAuthorityCouncilName = schoolDetails.LocalAuthorityName,
                LocalAuthorityWebsite = schoolDetails.LocalAuthorityWebsite,
                Latitude = latLong?.Latitude.ToString() ?? string.Empty,
                Longitude = latLong?.Longitude.ToString() ?? string.Empty,
                TypeOfSchool = schoolDetails.TypeOfSchool.ToDisplayField(),
                HeadTeacher = schoolDetails.HeadTeacher.ToDisplayField(),
                AgeRange = schoolDetails.AgeRange.ToDisplayField(),
                NumberOfPupils = (schoolDetails.NumberOfPupils?.ToInt()?.ToString("N0") ?? schoolDetails.NumberOfPupils).ToDisplayField(),
                PupilSex = schoolDetails.PupilSex.ToDisplayField(),
                ReligiousCharacter = schoolDetails.ReligiousCharacter.ToDisplayField(),
                SixthForm = GetSixthForm(schoolDetails.OfficialSixthFormId).ToDisplayField(),
                SenUnit = GetSenUnit(schoolDetails.ResourcedProvisionName),
                ResourcedProvision = GetResourcedProvision(schoolDetails.ResourcedProvisionName),
                IsLocalAuthoritySchool = schoolDetails.EstablishmentTypeGroupId.ToInt() == LocalAuthorityEstablishmentGroupTypeId,
                StatusCode = schoolDetails.Status,
                ClosedDate = schoolDetails.ClosedDate.ToDisplayField(),
                OpenDate = schoolDetails.OpenDate,
                OpenReasonId = schoolDetails.OpenReasonId,
                SenTypes = schoolDetails.SenTypes.ToDisplayField(),
                RecentlyOpenedSchoolMessage = GetRecentlyOpenedSchoolMessage(schoolDetails.OpenReasonId, schoolDetails.OpenDate),
                Predecessors = schoolDetails.Predecessors?.Select(p => SuccessorOrPredecessorDetailsModel.Map(p)).ToList(),
                Successors = schoolDetails.Successors?.Select(s => SuccessorOrPredecessorDetailsModel.Map(s)).ToList(),
                EducationPhase = EducationPhaseFormatter.Format(schoolDetails.IsKS2, schoolDetails.IsKS4, schoolDetails.IsKS5),
                IsKS4 = schoolDetails.IsKS4
            };
        }

        private static string GetSenUnit(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? NotRecorded
                : (string.Equals(value, SENUnit, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, ResourcedProvisionAndSENUnit, StringComparison.OrdinalIgnoreCase))
                ? Yes
                : No;
        }

        private static string GetResourcedProvision(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? NotRecorded
                : (string.Equals(value, ResourcedProvisionText, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, ResourcedProvisionAndSENUnit, StringComparison.OrdinalIgnoreCase))
                ? Yes
                : No;
        }

        private static string? GetSixthForm(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : string.Equals(value, "1") ? Yes : No;
        }

        private static DisplayField<string> GetRecentlyOpenedSchoolMessage(int? openReasonId, DateOnly? openDate)
        {
            if (!openDate.HasValue || !AcademicYearsHelper.IsWithinLastThreeAcademicYears(openDate.Value))
                return DisplayField<string>.NotAvailable();

            var date = $" on {openDate.Value:d MMMM yyyy}";
            return openReasonId switch
            {
                AcademyOpenReasonId => $"Opened as an academy{date}".ToDisplayField(),
                _ => $"This school opened{date}".ToDisplayField()
            };
        }
    }
}
