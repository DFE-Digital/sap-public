using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Repositories.SubjectEntries;

public interface IKs5EstablishmentSubjectEntriesRepository
{
    Task<IEnumerable<SubjectsEntered>> GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
