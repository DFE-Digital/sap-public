using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;

namespace SAPPub.Core.Services.KS4.AboutSchool;

public sealed class AboutSchoolService(
    IEstablishmentService establishmentService,
    ILaUrlsRepository laUrlsRepository,
    IEstablishmentLinksRepository establishmentLinksRepository) : IAboutSchoolService
{
    public async Task<AboutSchoolModel> GetAboutSchoolDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        var openDate = establishment.OpenDate.ToDateOnly();
        var links = openDate is not null
            ? AcademicYearsHelper.IsWithinLastThreeAcademicYears(openDate.Value)
                ? await establishmentLinksRepository.GetLinksAsync(urn, ct) : null
            : null;
        var predecessors = links?
            .Where(l => l.Urn is not null && l.LinkType is not null && l.LinkType.Contains("Predecessor"))
            .Select(l => new EstablishmentLink
            {
                Urn = l.LinkUrn!,
                Name = l.LinkName!,
            })
            .ToList();

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
            ResourcedProvisionName = establishment.ResourcedProvisionName,
            EstablishmentTypeGroupId = establishment.EstablishmentTypeGroupId,
            ClosedDate = establishment.ClosedDate.ToDateOnly(),
            Status = establishment.StatusCode.ToStatus(),
            OpenDate = establishment.OpenDate.ToDateOnly(),
            OpenReasonId = establishment.OpenReasonId,
            Predecessors = predecessors
        };
    }
}
