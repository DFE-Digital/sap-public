using SAPPub.Core.Entities.KS4.SubjectEntries;

namespace SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;

public interface IEstablishmentSubjectEntriesService
{
    public (EstablishmentCoreSubjectEntries, EstablishmentAdditionalSubjectEntries) GetSubjectEntriesByUrn(string urn);
}
