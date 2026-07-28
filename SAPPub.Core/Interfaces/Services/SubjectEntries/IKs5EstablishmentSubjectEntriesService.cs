using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.SubjectEntries;

public interface IKs5EstablishmentSubjectEntriesService
{
    Task<IEnumerable<SubjectsEntered>> GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
