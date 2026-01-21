using SAPPub.Core.Entities.KS4.SubjectEntries;

namespace SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;

public interface IEstablishmentSubjectEntriesRepository
{
    public EstablishmentCoreSubjectEntries GetCoreSubjectEntriesByUrn(string urn);

    public EstablishmentAdditionalSubjectEntries GetAdditionalSubjectEntriesByUrn(string urn);
}
