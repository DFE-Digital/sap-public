using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;

namespace SAPPub.Infrastructure.Repositories.KS4.SubjectEntries
{
    public sealed class EstablishmentSubjectEntriesRepository : IEstablishmentSubjectEntriesRepository
    {
        private readonly IGenericRepository<EstablishmentSubjectEntryRow> _repo;

        public EstablishmentSubjectEntriesRepository(IGenericRepository<EstablishmentSubjectEntryRow> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<EstablishmentCoreSubjectEntries> GetCoreSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
        {
            var rows = await GetAggregatedRowsAsync(urn, ct);

            var core = rows
                .Where(r => IsCoreSubject(r.Subject))
                .Select(r => new EstablishmentCoreSubjectEntries.SubjectEntry
                {
                    SubEntCore_Sub_Est_Current_Num = r.Subject,
                    SubEntCore_Qual_Est_Current_Num = r.Qualification,
                    SubEntCore_Entr_Est_Current_Num = r.TotalAchieving
                })
                .ToList();

            return new EstablishmentCoreSubjectEntries { SubjectEntries = core };
        }

        public async Task<EstablishmentAdditionalSubjectEntries> GetAdditionalSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
        {
            var rows = await GetAggregatedRowsAsync(urn, ct);

            var additional = rows
                .Where(r => !IsCoreSubject(r.Subject))
                .Select(r => new EstablishmentAdditionalSubjectEntries.SubjectEntry
                {
                    SubEntAdd_Sub_Est_Current_Num = r.Subject,
                    SubEntAdd_Qual_Est_Current_Num = r.Qualification,
                    SubEntAdd_Entr_Est_Current_Num = r.TotalAchieving
                })
                .ToList();

            return new EstablishmentAdditionalSubjectEntries { SubjectEntries = additional };
        }

        private async Task<List<AggregatedSubjectRow>> GetAggregatedRowsAsync(string urn, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return [];

            var raw = await _repo.ReadManyAsync(new { Urn = urn }, ct);

            // One row per (Subject, Qualification) with summed counts across grades
            return raw
                .Where(r => !string.IsNullOrWhiteSpace(r.subject))
                .GroupBy(r => new
                {
                    Subject = r.subject!.Trim(),
                    Qualification = (r.qualification_type ?? r.qualification_detailed ?? string.Empty).Trim()
                })
                .Select(g => new AggregatedSubjectRow(
                    Subject: g.Key.Subject,
                    Qualification: string.IsNullOrWhiteSpace(g.Key.Qualification) ? null : g.Key.Qualification,
                    TotalAchieving: g.Sum(x => x.number_achieving ?? 0)
                ))
                .OrderBy(x => x.Subject)
                .ThenBy(x => x.Qualification)
                .ToList();
        }

        private static readonly HashSet<string> CoreSubjects = new(StringComparer.OrdinalIgnoreCase)
        {
            // English
            "English Language",
            "English Literature",

            // Maths
            "Mathematics",
            "Mathematics AS level",
            "Statistics",

            // Science
            "Biology",
            "Chemistry",
            "Physics",
            "Combined Science",
            "Computer Science",
            "Other Sciences"
        };

        private static bool IsCoreSubject(string? subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                return false;

            return CoreSubjects.Contains(subject.Trim());
        }

        private sealed record AggregatedSubjectRow(string Subject, string? Qualification, int TotalAchieving);
    }
}
