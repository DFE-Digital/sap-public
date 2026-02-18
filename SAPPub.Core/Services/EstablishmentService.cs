using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public sealed class EstablishmentService : IEstablishmentService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILookupService _lookUpService;

        public EstablishmentService(
            IEstablishmentRepository establishmentRepository,
            ILookupService lookUpService)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _lookUpService = lookUpService ?? throw new ArgumentNullException(nameof(lookUpService));
        }

        public async Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default)
        {
            return await _establishmentRepository.GetAllEstablishmentsAsync(ct);
        }

        public async Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new Establishment();

            var establishment = await _establishmentRepository.GetEstablishmentAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(establishment.URN))
                return new Establishment();

            // Prefer an async lookup call if lookups come from the DB.
            var allLookups = await _lookUpService.GetAllLookupsAsync(ct);

            establishment.TypeOfEstablishmentName = GetLookupByCode(allLookups, "TypeOfEstablishment", establishment.TypeOfEstablishmentId);
            establishment.AdmissionPolicy = GetLookupByCode(allLookups, "AdmissionsPolicy", establishment.AdmissionsPolicyId);
            establishment.DistrictAdministrativeName = GetLookupByCode(allLookups, "DistrictAdministrative", establishment.DistrictAdministrativeId);
            establishment.PhaseOfEducationName = GetLookupByCode(allLookups, "PhaseOfEducation", establishment.PhaseOfEducationId);
            establishment.GenderName = GetLookupByCode(allLookups, "Gender", establishment.GenderId);
            establishment.ReligiousCharacterName = GetLookupByCode(allLookups, "ReligiousCharacter", establishment.ReligiousCharacterId);
            establishment.UrbanRuralName = GetLookupByCode(allLookups, "UrbanRural", establishment.UrbanRuralId);
            establishment.TrustName = GetLookupByCode(allLookups, "Trusts", establishment.TrustsId);
            establishment.LAName = GetLookupByCode(allLookups, "LA", establishment.LAId);

            return establishment;
        }

        private static string GetLookupByCode(IEnumerable<Lookup> lookups, string type, string? id)
        {
            return lookups.FirstOrDefault(x => x.LookupType == type && x.Id == id)?.Name ?? string.Empty;
        }
    }
}
