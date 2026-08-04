using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Infrastructure.Repositories.Performance;

public sealed class KS5EstablishmentSubjectEntriesRepository(IGenericRepository<KS5EstablishmentSubjectEntryRow> repo) : IKS5EstablishmentSubjectEntriesRepository
{
    private const string TotalExamEntriesRowIndicator = "Total exam entries";

    private readonly IGenericRepository<KS5EstablishmentSubjectEntryRow> _repo = repo ?? throw new ArgumentNullException(nameof(repo));

    public async Task<IEnumerable<SubjectsEnteredModel>> GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
        {
            return [];
        }

        ct.ThrowIfCancellationRequested();

        var rows = (await _repo.ReadManyAsync(new { Urn = urn }, ct)).Where(a => a.grade == TotalExamEntriesRowIndicator && a.subject != "All subjects");
        
        if (rows is null || !rows.Any())
        {
            return [];
        }

        return [.. rows
            .Select(r => new SubjectsEnteredModel
            {
                Subject = r.subject?.Trim(),
                Qualification = r.qualification_detailed,
                TotalNumberOfEntries = r.entries_count,
                Level = r.qualification_level,
                ExamCohort = r.exam_cohort
            })
            .OrderBy(r => r.Subject)
            .ThenBy(r => r.Qualification)];
    }
}