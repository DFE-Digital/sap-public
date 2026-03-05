namespace SAPPub.Core.ServiceModels.KS4.AboutSchool;

public class AboutSchoolModel
{
    public required string Urn { get; set; }

    public required string SchoolName { get; set; }

    public string? AcademyTrust { get; set; }

    public string? AcademyTrustUpdatedIn { get; set; }

    public string? Website { get; set; }

    public string? Telephone { get; set; }

    public string? Address { get; set; }

    public string? LocalAuthority { get; set; }

    public string? LocalAuthorityName { get; set; }

    public string? LocalAuthorityWebsite { get; set; }

    public string Easting { get; set; } = string.Empty;

    public string Northing { get; set; } = string.Empty;

    public string? TypeOfSchool { get; set; }

    public string? HeadTeacher { get; set; }

    public string? AgeRange { get; set; }

    public string? NumberOfPupils { get; set; }

    public string? PupilSex { get; set; }

    public string? ReligiousCharacter { get; set; }

    public string OfficialSixthFormId { get; set; } = string.Empty;

    public string ResourcedProvision { get; set; } = string.Empty;
}
