using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;

public interface IEstablishmentSubjectEntriesService
{
    Task<(IEnumerable<SubjectsEntered> Gcse, IEnumerable<SubjectsEntered> Vocational, IEnumerable<SubjectsEntered> Other)>
        GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
