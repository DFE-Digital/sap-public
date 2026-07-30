using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Repositories.SubjectEntries;

public interface IKS4EstablishmentSubjectEntriesRepository
{
    Task<IEnumerable<SubjectsEnteredModel>> GetGcseSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<SubjectsEnteredModel>> GetVocationalAwardSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    Task<IEnumerable<SubjectsEnteredModel>> GetOtherSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
