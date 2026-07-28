using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.SubjectEntries;

public interface IKs4EstablishmentSubjectEntriesService
{
    Task<(IEnumerable<SubjectsEntered> Gcse, IEnumerable<SubjectsEntered> Vocational, IEnumerable<SubjectsEntered> Other)>
        GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
