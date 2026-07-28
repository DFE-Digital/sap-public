using SAPPub.Core.Entities.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Infrastructure.Repositories.SubjectEntries;

public sealed class Ks5EstablishmentSubjectEntriesRepository(IGenericRepository<EstablishmentKs5SubjectEntryRow> repo) : IKs5EstablishmentSubjectEntriesRepository
{
    private const string TotalExamEntriesRowIndicator = "Total exam entries";

    private readonly IGenericRepository<EstablishmentKs5SubjectEntryRow> _repo = repo ?? throw new ArgumentNullException(nameof(repo));

    public async Task<IEnumerable<SubjectsEntered>> GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
        {
            return [];
        }

        ct.ThrowIfCancellationRequested();

        var rows = (await _repo.ReadManyAsync(new { Urn = urn }, ct)).Where(a => a.grade == TotalExamEntriesRowIndicator);

        if (rows is null || !rows.Any())
        {
            return [];
        }

        return [.. rows
            .Select(r => new SubjectsEntered
            {
                Subject = r.subject?.Trim(),
                Qualification = r.qualification_detailed,
                TotalNumberOfEntries = r.entries_count
            })
            .OrderBy(r => r.Subject)
            .ThenBy(r => r.Qualification)];
    }
}