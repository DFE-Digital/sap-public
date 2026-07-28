using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;

public interface IEstablishmentSubjectEntriesRepository
{
    Task<IEnumerable<SubjectsEntered>> GetGcseSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<SubjectsEntered>> GetVocationalAwardSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<SubjectsEntered>> GetOtherSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
