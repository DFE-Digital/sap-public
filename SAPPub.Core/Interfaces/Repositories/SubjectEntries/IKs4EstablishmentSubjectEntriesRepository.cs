using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Repositories.SubjectEntries;

public interface IKs4EstablishmentSubjectEntriesRepository
{
    Task<IEnumerable<SubjectsEntered>> GetGcseSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<SubjectsEntered>> GetVocationalAwardSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<SubjectsEntered>> GetOtherSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
