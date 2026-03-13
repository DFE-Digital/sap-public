using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;

namespace SAPPub.Core.Services.KS4.AboutSchool;

public sealed class AboutSchoolService(
    IEstablishmentService establishmentService,
    ILaUrlsRepository laUrlsRepository) : IAboutSchoolService
{
    public async Task<AboutSchoolModel> GetAboutSchoolDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        var laUrls = !string.IsNullOrWhiteSpace(establishment.GSSLACode) ? await laUrlsRepository.GetLaAsync(establishment.GSSLACode, ct) : null;

        return new AboutSchoolModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            AcademyTrust = establishment.TrustName,
            Website = establishment.Website,
            Telephone = establishment.TelephoneNum,
            Address = establishment.Address,
            LocalAuthority = establishment.LAName,
            LocalAuthorityName = laUrls?.Name,
            LocalAuthorityWebsite = laUrls?.LAMainUrl,
            Easting = establishment.Easting,
            Northing = establishment.Northing,
            TypeOfSchool = establishment.TypeOfEstablishmentName,
            HeadTeacher = establishment.Headteacher,
            AgeRange = establishment.AgeRange,
            NumberOfPupils = establishment.TotalPupils,
            PupilSex = establishment.GenderName,
            ReligiousCharacter = establishment.ReligiousCharacterName,
            OfficialSixthFormId = establishment.OfficialSixthFormId,
            ResourcedProvision = establishment.ResourcedProvision,
            EstablishmentTypeGroupId = establishment.EstablishmentTypeGroupId,
        };
    }
}
