using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS4EstablishmentSubjectEntriesService
{
    Task<(IEnumerable<SubjectsEnteredModel> Gcse, IEnumerable<SubjectsEnteredModel> Vocational, IEnumerable<SubjectsEnteredModel> Other)>
        GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
