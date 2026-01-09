using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public class EstablishmentService(
        IEstablishmentRepository establishmentRepository,
        ILookupService lookUpService) : IEstablishmentService
    {
        private readonly IEstablishmentRepository _establishmentRepository = establishmentRepository;
        private readonly ILookupService _lookUpService = lookUpService;

        public IEnumerable<Establishment> GetAllEstablishments()
        {
            return _establishmentRepository.GetAllEstablishments();
        }

        public Establishment GetEstablishment(string urn)
        {
            var establishment = _establishmentRepository.GetEstablishment(urn);
            if (!string.IsNullOrWhiteSpace(establishment?.URN))
            {
                var allLookups = _lookUpService.GetAllLookups();
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

            throw new Exception("Error in GetEstablishment");
        }

        private static string GetLookupByCode(IEnumerable<Lookup> lookups, string type, string? id)
        {
            return lookups.FirstOrDefault(x => x.LookupType == type && x.Id == id)?.Name ?? string.Empty;
        }
    }
}
