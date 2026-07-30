using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.Performance;

public sealed class KS4EstablishmentSubjectEntriesService(IKS4EstablishmentSubjectEntriesRepository subjectEntriesRepository) : IKS4EstablishmentSubjectEntriesService
{
    private readonly IKS4EstablishmentSubjectEntriesRepository _repo = subjectEntriesRepository ?? throw new ArgumentNullException(nameof(subjectEntriesRepository));

    public async Task<(IEnumerable<SubjectsEnteredModel> Gcse, IEnumerable<SubjectsEnteredModel> Vocational, IEnumerable<SubjectsEnteredModel> Other)>
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