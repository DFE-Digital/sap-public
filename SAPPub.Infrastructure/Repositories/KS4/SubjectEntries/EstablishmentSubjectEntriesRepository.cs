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
                    SubEntCore_Entr_Est_Current_Num = r.TotalEnteredPercent
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
                    SubEntAdd_Entr_Est_Current_Num = r.TotalEnteredPercent
                })
                .ToList();

            return new EstablishmentAdditionalSubjectEntries { SubjectEntries = additional };
        }

        private async Task<List<AggregatedSubjectRow>> GetAggregatedRowsAsync(string urn, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return [];

            var raw = (await _repo.ReadManyAsync(new { Urn = urn }, ct)).ToList();
            if (raw.Count == 0)
                return [];

            // Cohort size should be consistent; use max defensively.
            var cohort = raw.Select(r => r.pupil_count ?? 0).Max();
            if (cohort <= 0)
                return [];

            return raw
                .Where(r => !string.IsNullOrWhiteSpace(r.subject))
                .GroupBy(r => new
                {
                    Subject = r.subject!.Trim(), // subject only (no subject_discount_group)
                    Qualification = (r.qualification_type ?? r.qualification_detailed ?? string.Empty).Trim()
                })
                .Select(g =>
                {
                    var totalEntries = g.Sum(x => x.number_achieving ?? 0);

                    // Convert summed grade counts to percent-of-cohort.
                    var pct = (totalEntries / (double)cohort) * 100.0;

                    // Safety clamp (optional but sensible with dirty data)
                    if (pct > 100.0) pct = 100.0;
                    if (pct < 0.0) pct = 0.0;

                    return new AggregatedSubjectRow(
                        Subject: g.Key.Subject,
                        Qualification: string.IsNullOrWhiteSpace(g.Key.Qualification) ? null : g.Key.Qualification,
                        TotalEnteredPercent: pct
                    );
                })
                .OrderBy(x => x.Subject)
                .ThenBy(x => x.Qualification)
                .ToList();
        }

        private sealed record AggregatedSubjectRow(string Subject, string? Qualification, double TotalEnteredPercent);

        private static readonly HashSet<string> CoreSubjects = new(StringComparer.OrdinalIgnoreCase)
        {
            "English Language",
            "English Literature",
            "Mathematics",
            "Mathematics AS level",
            "Statistics",
            "Biology",
            "Chemistry",
            "Physics",
            "Combined Science",
            "Other Sciences"
        };

        private static bool IsCoreSubject(string? subject)
            => !string.IsNullOrWhiteSpace(subject) && CoreSubjects.Contains(subject.Trim());
    }
}
