using System.Reflection;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Tests.UI.Infrastructure;

public sealed class FakeGenericRepository<T> : IGenericRepository<T> where T : class
{
    private static readonly Dictionary<string, Establishment> Establishments = new(StringComparer.OrdinalIgnoreCase)
    {
        ["143034"] = new Establishment
        {
            URN = "143034",
            EstablishmentName = "St Paul's Church of England Academy",
            LAId = "845",
            LAName = "East Sussex",
            EstablishmentNumber = "3090",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary"
        },

        ["100273"] = new Establishment
        {
            URN = "100273",
            EstablishmentName = "Saint Paul Roman Catholic Infant School",
            LAId = "204",
            LAName = "Hackney",
            EstablishmentNumber = "3658",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary"
        },

        ["102848"] = new Establishment
        {
            URN = "102848",
            EstablishmentName = "SS Peter and Paul's Catholic Primary School",
            LAId = "317",
            LAName = "Redbridge",
            EstablishmentNumber = "3513",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary"
        },


        ["105574"] = new Establishment
        {
            URN = "105574",
            EstablishmentName = "Loreto High School Chorlton",
            LAId = "999",
            LAName = "Test LA",
            EstablishmentNumber = "9999",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary"
        }
    };

    public Task<T?> ReadAsync(string id, CancellationToken ct = default)
        => ReadSingleAsync(new { Id = id }, ct);

    public Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default)
        => Task.FromResult(Enumerable.Empty<T>());

    public Task<T?> ReadSingleAsync(object? parameters, CancellationToken ct = default)
    {
        if (parameters is null) return Task.FromResult<T?>(default);

        if (typeof(T) == typeof(Establishment))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && Establishments.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);

            return Task.FromResult<T?>((T)(object)new Establishment { URN = id ?? string.Empty });
        }

        return Task.FromResult<T?>(default);
    }

    public Task<IEnumerable<T>> ReadManyAsync(object? parameters, CancellationToken ct = default)
    {
        if (parameters is null) return Task.FromResult(Enumerable.Empty<T>());

        if (typeof(T) == typeof(EstablishmentSubjectEntryRow))
        {
            var urn = GetPropertyString(parameters, "Urn");
            if (string.IsNullOrWhiteSpace(urn))
                return Task.FromResult(Enumerable.Empty<T>());

            // Must be consistent and >0 or your aggregation returns empty
            var cohort = 100;

            var rows = new List<EstablishmentSubjectEntryRow>
            {
                // Core
                MakeRow(urn, cohort, "English Language", "GCSE", 30),
                MakeRow(urn, cohort, "Mathematics", "GCSE", 35),
                MakeRow(urn, cohort, "Combined Science", "GCSE", 40),
                MakeRow(urn, cohort, "Computer Science", "GCSE", 12),

                // A few more core
                MakeRow(urn, cohort, "Biology", "GCSE", 20),
                MakeRow(urn, cohort, "Chemistry", "GCSE", 18),
                MakeRow(urn, cohort, "Physics", "GCSE", 15),
            };

            // Additional (enough to trigger pagination)
            var additionalSubjects = new[]
            {
                "History", "Geography", "French", "Spanish", "Art", "Music", "Drama",
                "PE", "Business", "Design & Technology", "RS", "Media Studies",
                "Sociology", "Psychology", "Citizenship"
            };

            rows.AddRange(additionalSubjects.Select(s => MakeRow(urn, cohort, s, "GCSE", 8)));

            return Task.FromResult(rows.Cast<T>());
        }

        return Task.FromResult(Enumerable.Empty<T>());
    }

    private static EstablishmentSubjectEntryRow MakeRow(string urn, int cohort, string subject, string qual, int count)
        => new()
        {
            school_urn = urn,
            pupil_count = cohort,
            subject = subject,
            qualification_type = qual,
            qualification_detailed = null,
            grade = "All",
            number_achieving = count
        };

    private static string? GetPropertyString(object obj, string name)
    {
        var prop = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        return prop?.GetValue(obj)?.ToString();
    }
}
