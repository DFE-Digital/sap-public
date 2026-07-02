using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
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
    ILAService laService,
    IEstablishmentLinksRepository establishmentLinksRepository) : IAboutSchoolService
{
    public async Task<AboutSchoolModel> GetAboutSchoolDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        var openDate = establishment.OpenDate.ToDateOnly();

        bool findSuccessorLinks = establishment.StatusCode.ToStatus() == EstablishmentStatus.Closed;
        bool findPredecessorLinks = openDate is not null && AcademicYearsHelper.IsWithinLastThreeAcademicYears(openDate.Value);

        var links = findPredecessorLinks || findSuccessorLinks
             ? await establishmentLinksRepository.GetLinksAsync(urn, ct)
             : null;

        var predecessorLinks = (findPredecessorLinks && links != null && links.Any())
            ? links?
                .Where(l => l.Urn is not null && l.LinkType is not null && l.LinkType.Contains("Predecessor"))
                .Select(l => new EstablishmentLinkModel
                {
                    Urn = l.LinkUrn!,
                    Name = l.LinkName!,
                })
                .ToList()
            : null;
        var sucessorLinks = (findSuccessorLinks && links != null && links.Any())
            ? links?
                .Where(l => l.Urn is not null && l.LinkType is not null && l.LinkType.Contains("Successor"))
                .Select(l => new EstablishmentLinkModel
                {
                    Urn = l.LinkUrn!,
                    Name = l.LinkName!,
                })
                .ToList()
            : null;

        LaUrls? laUrls = await laService.GetLaUrlsAsync(establishment, ct);

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
            SenTypes = establishment.SenTypes,
            Predecessors = predecessorLinks,
            Successors = sucessorLinks,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5
        };
    }

    public async Task<IEnumerable<AboutSchoolComparisonModel>> GetAboutSchoolForComparisonAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        var establishments = await establishmentService.GetEstablishmentsAsync(urns, ct);
        var laUrlsList = await laService.GetLaUrlsListForEstablishmentsAsync(establishments, ct);

        return 
            establishments.Select(est =>
            {
                var laUrls = laUrlsList?.FirstOrDefault(a => a!.Id == est.GSSLACode || a.Id == est.DistrictAdministrativeId);
                
                return new AboutSchoolComparisonModel
                {
                    Urn = est.URN,
                    SchoolName = est.EstablishmentName,
                    Address = est.Address,
                    Website = est.Website,
                    Easting = est.Easting,
                    Northing = est.Northing,
                    LocalAuthority = est.LAName,
                    LocalAuthorityName = laUrls?.Name,
                    LocalAuthorityWebsite = laUrls?.LAMainUrl,
                };
            }).OrderBy(a => a.SchoolName);
    }
}