using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.Interfaces.Services.SubjectEntries;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.SubjectEntries;

public sealed class Ks5EstablishmentSubjectEntriesService(IKs5EstablishmentSubjectEntriesRepository subjectEntriesRepository) : IKs5EstablishmentSubjectEntriesService
{
    private readonly IKs5EstablishmentSubjectEntriesRepository _repo = subjectEntriesRepository ?? throw new ArgumentNullException(nameof(subjectEntriesRepository));

    Task<IEnumerable<SubjectsEntered>> IKs5EstablishmentSubjectEntriesService.GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct)
    { 
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        return _repo.GetSubjectEntriesByUrnAsync(urn, ct);
    }
}