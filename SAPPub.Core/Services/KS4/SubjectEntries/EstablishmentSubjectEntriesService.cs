using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.SubjectEntries;

public sealed class EstablishmentSubjectEntriesService(IEstablishmentSubjectEntriesRepository subjectEntriesRepository) : IEstablishmentSubjectEntriesService
{
    private readonly IEstablishmentSubjectEntriesRepository _repo = subjectEntriesRepository ?? throw new ArgumentNullException(nameof(subjectEntriesRepository));

    public async Task<(IEnumerable<SubjectsEntered> Gcse, IEnumerable<SubjectsEntered> Vocational, IEnumerable<SubjectsEntered> Other)>
        GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        var gcseTask = _repo.GetGcseSubjectEntriesByUrnAsync(urn, ct);
        var vocationalTask = _repo.GetVocationalAwardSubjectEntriesByUrnAsync(urn, ct);
        var otherTask = _repo.GetOtherSubjectEntriesByUrnAsync(urn, ct);

        await Task.WhenAll(gcseTask, vocationalTask, otherTask);

        return (await gcseTask, await vocationalTask, await otherTask);
    }
}