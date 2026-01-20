using SAPPub.Core.Entities.KS4.SubjectEntries;

namespace SAPPub.Core.Interfaces.Repositories;

public interface IEstablishmentSubjectEntriesRepository
{
    public EstablishmentCoreSubjectEntries GetCoreSubjectEntriesByUrn(string urn);

    public EstablishmentAdditionalSubjectEntries GetAdditionalSubjectEntriesByUrn(string urn);
}
