using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;

namespace SAPPub.Core.Services.KS4.SubjectEntries;

public class EstablishmentSubjectEntriesService(IEstablishmentSubjectEntriesRepository subjectEntriesRepository) : IEstablishmentSubjectEntriesService
{
    public (EstablishmentCoreSubjectEntries, EstablishmentAdditionalSubjectEntries) GetSubjectEntriesByUrn(string urn)
    {
        var coreSubjectEntries = subjectEntriesRepository.GetCoreSubjectEntriesByUrn(urn);
        var additionalSubjectEntries = subjectEntriesRepository.GetAdditionalSubjectEntriesByUrn(urn);
        return (coreSubjectEntries, additionalSubjectEntries);
    }
}
