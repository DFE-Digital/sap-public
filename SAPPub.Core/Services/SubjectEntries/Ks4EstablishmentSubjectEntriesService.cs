using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.Interfaces.Services.SubjectEntries;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.SubjectEntries;

public sealed class Ks4EstablishmentSubjectEntriesService(IKs4EstablishmentSubjectEntriesRepository subjectEntriesRepository) : IKs4EstablishmentSubjectEntriesService
{
    private readonly IKs4EstablishmentSubjectEntriesRepository _repo = subjectEntriesRepository ?? throw new ArgumentNullException(nameof(subjectEntriesRepository));

    public async Task<(IEnumerable<SubjectsEntered> Gcse, IEnumerable<SubjectsEntered> Vocational, IEnumerable<SubjectsEntered> Other)>
        GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var gcseTask = _repo.GetGcseSubjectEntriesByUrnAsync(urn, ct);
        var vocationalTask = _repo.GetVocationalAwardSubjectEntriesByUrnAsync(urn, ct);
        var otherTask = _repo.GetOtherSubjectEntriesByUrnAsync(urn, ct);

        await Task.WhenAll(gcseTask, vocationalTask, otherTask);

        return (await gcseTask, await vocationalTask, await otherTask);
    }
}