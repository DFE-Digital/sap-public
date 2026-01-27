using SAPData.Models;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SAPData;

public sealed class GenerateViews
{
    private readonly IReadOnlyList<DataMapRow> _rows;
    private readonly string _tableMappingPath;
    private readonly string _sqlDir;

    // raw_sources.json path in repo
    private static readonly string[] RawSourcesCandidates =
    {
        "raw_sources.json",
        Path.Combine("SAPData", "raw_sources.json"),
        Path.Combine(AppContext.BaseDirectory, "raw_sources.json"),
        Path.Combine(AppContext.BaseDirectory, "SAPData", "raw_sources.json")
    };

    private sealed record ViewSpec(string ViewName, string Range, string Type);

    private sealed record RawSource(
        string Type,
        string Subtype,
        string Year,
        string SourceOrg,
        string FileName
    );

    private static readonly ViewSpec[] Views =
    {
        new("v_establishment", "Establishment", "Establishment"),
        new("v_establishment_links", "Establishment", "Establishment"),
        new("v_establishment_group_links", "Establishment", "Establishment"),
        new("v_establishment_subject_entries", "Establishment", "KS4_Performance"),

        new("v_establishment_absence", "Establishment", "PupilAbsence"),
        new("v_establishment_destinations", "Establishment", "KS4_Destinations"),
        new("v_establishment_performance", "Establishment", "KS4_Performance"),
        new("v_establishment_workforce", "Establishment", "Workforce"),

        new("v_england_destinations", "England", "KS4_Destinations"),
        new("v_england_performance", "England", "KS4_Performance"),

        new("v_la_destinations", "LA", "KS4_Destinations"),
        new("v_la_performance", "LA", "KS4_Performance"),
        new("v_la_subject_entries", "LA", "KS4_Performance")
    };

    public GenerateViews(IReadOnlyList<DataMapRow> rows, string tableMappingPath, string sqlDir)
    {
        _rows = rows;
        _tableMappingPath = tableMappingPath;
        _sqlDir = sqlDir;
    }

    public void Run()
    {
        Directory.CreateDirectory(_sqlDir);

        var tableMap = LoadTableMappings();
        var sources = LoadRawSources();

        foreach (var view in Views)
        {
            string sql;

            // 1) Establishment dimension (GIAS edubasealldataYYYYmmDD)
            if (view.ViewName.Equals("v_establishment", StringComparison.OrdinalIgnoreCase))
            {
                if (!TryResolveManagedDatasetKey(
                        sources,
                        tableMap,
                        sourceOrg: "GIAS",
                        type: "All establishment",
                        subtype: "Metadata",
                        year: "Current",
                        out var datasetKey))
                {
                    sql = BuildSkippedSql(view.ViewName, "Could not resolve dataset key from raw_sources.json (GIAS/All establishment/Metadata/Current).");
                    Write(view.ViewName, sql);
                    continue;
                }

                if (!TryResolveRawTable(tableMap, datasetKey, out var rawTable))
                {
                    sql = BuildSkippedSql(view.ViewName, $"Could not resolve raw table mapping for datasetKey='{datasetKey}'.");
                    Write(view.ViewName, sql);
                    continue;
                }

                sql = GenerateEstablishmentDimensionView(rawTable);
            }

            // 2) Mirror view (GIAS: all establishment links)
            else if (view.ViewName.Equals("v_establishment_links", StringComparison.OrdinalIgnoreCase))
            {
                if (!TryResolveManagedDatasetKey(
                        sources,
                        tableMap,
                        sourceOrg: "GIAS",
                        type: "All establishment",
                        subtype: "Links",
                        year: "Current",
                        out var datasetKey))
                {
                    sql = BuildSkippedSql(view.ViewName, "Could not resolve dataset key from raw_sources.json (GIAS/All establishment/Links/Current).");
                    Write(view.ViewName, sql);
                    continue;
                }

                if (!TryResolveRawTable(tableMap, datasetKey, out var rawTable))
                {
                    sql = BuildSkippedSql(view.ViewName, $"Could not resolve raw table mapping for datasetKey='{datasetKey}'.");
                    Write(view.ViewName, sql);
                    continue;
                }

                sql = GenerateMirrorMaterializedView(view.ViewName, rawTable);
            }

            // 3) Mirror view (GIAS: academy sponsor/trust links)
            else if (view.ViewName.Equals("v_establishment_group_links", StringComparison.OrdinalIgnoreCase))
            {
                if (!TryResolveManagedDatasetKey(
                        sources,
                        tableMap,
                        sourceOrg: "GIAS",
                        type: "Academy sponsor and trust",
                        subtype: "Links",
                        year: "Current",
                        out var datasetKey))
                {
                    sql = BuildSkippedSql(view.ViewName, "Could not resolve dataset key from raw_sources.json (GIAS/Academy sponsor and trust/Links/Current).");
                    Write(view.ViewName, sql);
                    continue;
                }

                if (!TryResolveRawTable(tableMap, datasetKey, out var rawTable))
                {
                    sql = BuildSkippedSql(view.ViewName, $"Could not resolve raw table mapping for datasetKey='{datasetKey}'.");
                    Write(view.ViewName, sql);
                    continue;
                }

                sql = GenerateMirrorMaterializedView(view.ViewName, rawTable);
            }

            // 4) Mirror view (EES: SubjectEntries_2 = school / establishment subject entries)
            else if (view.ViewName.Equals("v_establishment_subject_entries", StringComparison.OrdinalIgnoreCase))
            {
                if (!TryResolveManagedDatasetKey(
                        sources,
                        tableMap,
                        sourceOrg: "EES",
                        type: "KS4_Performance",
                        subtype: "SubjectEntries_2",
                        year: "Current",
                        out var datasetKey))
                {
                    sql = BuildSkippedSql(view.ViewName, "Could not resolve dataset key from raw_sources.json (EES/KS4_Performance/SubjectEntries_2/Current).");
                    Write(view.ViewName, sql);
                    continue;
                }

                if (!TryResolveRawTable(tableMap, datasetKey, out var rawTable))
                {
                    sql = BuildSkippedSql(view.ViewName, $"Could not resolve raw table mapping for datasetKey='{datasetKey}'.");
                    Write(view.ViewName, sql);
                    continue;
                }

                sql = GenerateMirrorMaterializedView(view.ViewName, rawTable);
            }

            // 5) Mirror view (EES: SubjectEntries = LA subject entries)
            else if (view.ViewName.Equals("v_la_subject_entries", StringComparison.OrdinalIgnoreCase))
            {
                if (!TryResolveManagedDatasetKey(
                        sources,
                        tableMap,
                        sourceOrg: "EES",
                        type: "KS4_Performance",
                        subtype: "SubjectEntries",
                        year: "Current",
                        out var datasetKey))
                {
                    sql = BuildSkippedSql(view.ViewName, "Could not resolve dataset key from raw_sources.json (EES/KS4_Performance/SubjectEntries/Current).");
                    Write(view.ViewName, sql);
                    continue;
                }

                if (!TryResolveRawTable(tableMap, datasetKey, out var rawTable))
                {
                    sql = BuildSkippedSql(view.ViewName, $"Could not resolve raw table mapping for datasetKey='{datasetKey}'.");
                    Write(view.ViewName, sql);
                    continue;
                }

                sql = GenerateMirrorMaterializedView(view.ViewName, rawTable);
            }

            // 6) Everything else uses DataMap-driven materialized view generation
            else
            {
                var viewRows = _rows
                    .Where(r => r.Range == view.Range)
                    .Where(r => r.Type == view.Type)
                    .Where(r => !string.IsNullOrWhiteSpace(r.PropertyName))
                    .Where(r => !IsIgnored(r))
                    .ToList();

                var ignoredRows = _rows
                    .Where(r => r.Range == view.Range)
                    .Where(r => r.Type == view.Type)
                    .Where(r => !string.IsNullOrWhiteSpace(r.PropertyName))
                    .Where(IsIgnored)
                    .ToList();

                if (ignoredRows.Count > 0)
                {
                    Console.WriteLine($"Ignoring {ignoredRows.Count} DataMap rows for {view.ViewName}");
                    foreach (var row in ignoredRows)
                        Console.WriteLine($"Ignored mapping: {row.Ref} ({row.PropertyName})");
                }

                if (viewRows.Count == 0)
                {
                    sql = BuildSkippedSql(view.ViewName, $"No DataMap rows found for Range='{view.Range}', Type='{view.Type}'.");
                    Write(view.ViewName, sql);
                    continue;
                }

                sql = GenerateMaterializedView(view.ViewName, viewRows, tableMap);
            }

            Write(view.ViewName, sql);
            Console.WriteLine($"Generated {view.ViewName}");
        }
    }

    private void Write(string viewName, string sql)
    {
        File.WriteAllText(
            Path.Combine(_sqlDir, $"03_{viewName}.sql"),
            sql,
            new UTF8Encoding(false));
    }

    private static string BuildSkippedSql(string viewName, string reason)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"-- AUTO-GENERATED MATERIALIZED VIEW: {viewName}");
        sb.AppendLine("-- NOTE: This file was generated but the view SQL was skipped.");
        sb.AppendLine($"-- REASON: {reason}");
        sb.AppendLine();
        sb.AppendLine($"-- DROP MATERIALIZED VIEW IF EXISTS {viewName};");
        sb.AppendLine($"-- CREATE MATERIALIZED VIEW {viewName} AS");
        sb.AppendLine($"-- SELECT NULL::text AS \"Skipped\";");
        return sb.ToString();
    }

    // =====================================================
    // ESTABLISHMENT DIMENSION (curated)
    // =====================================================

    private static string GenerateEstablishmentDimensionView(string? rawTable)
    {
        var sb = new StringBuilder();

        sb.AppendLine("-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment");
        sb.AppendLine();
        sb.AppendLine("DROP MATERIALIZED VIEW IF EXISTS v_establishment CASCADE;");
        sb.AppendLine();
        sb.AppendLine("CREATE MATERIALIZED VIEW v_establishment AS");
        sb.AppendLine("SELECT");
        sb.AppendLine("    t.\"URN\"                                  AS \"URN\",");
        sb.AppendLine("    t.\"LA (code)\"                            AS \"LAId\",");
        sb.AppendLine("    t.\"LA (name)\"                            AS \"LAName\",");
        sb.AppendLine("    clean_int(t.\"GOR (code)\")                AS \"RegionId\",");
        sb.AppendLine("    t.\"GOR (name)\"                           AS \"RegionName\",");
        sb.AppendLine("    t.\"EstablishmentName\"                    AS \"EstablishmentName\",");
        sb.AppendLine("    clean_int(t.\"EstablishmentNumber\")       AS \"EstablishmentNumber\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"Trusts (code)\")             AS \"TrustsId\",");
        sb.AppendLine("    t.\"Trusts (name)\"                        AS \"TrustName\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"AdmissionsPolicy (code)\")   AS \"AdmissionsPolicyId\",");
        sb.AppendLine("    t.\"AdmissionsPolicy (name)\"              AS \"AdmissionPolicy\",");
        sb.AppendLine();
        sb.AppendLine("    t.\"DistrictAdministrative (code)\"        AS \"DistrictAdministrativeId\",");
        sb.AppendLine("    t.\"DistrictAdministrative (name)\"        AS \"DistrictAdministrativeName\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"PhaseOfEducation (code)\")   AS \"PhaseOfEducationId\",");
        sb.AppendLine("    t.\"PhaseOfEducation (name)\"              AS \"PhaseOfEducationName\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"Gender (code)\")             AS \"GenderId\",");
        sb.AppendLine("    t.\"Gender (name)\"                        AS \"GenderName\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"OfficialSixthForm (code)\")  AS \"OfficialSixthFormId\",");
        sb.AppendLine("    clean_int(t.\"ReligiousCharacter (code)\") AS \"ReligiousCharacterId\",");
        sb.AppendLine("    t.\"ReligiousCharacter (name)\"            AS \"ReligiousCharacterName\",");
        sb.AppendLine();
        sb.AppendLine("    t.\"TelephoneNum\"                         AS \"TelephoneNum\",");
        sb.AppendLine("    clean_int(t.\"NumberOfPupils\")            AS \"TotalPupils\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"TypeOfEstablishment (code)\") AS \"TypeOfEstablishmentId\",");
        sb.AppendLine("    t.\"TypeOfEstablishment (name)\"            AS \"TypeOfEstablishmentName\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"ResourcedProvisionOnRoll\")  AS \"ResourcedProvision\",");
        sb.AppendLine("    t.\"TypeOfResourcedProvision (name)\"      AS \"ResourcedProvisionName\",");
        sb.AppendLine();
        sb.AppendLine("    clean_int(t.\"UKPRN\")                     AS \"UKPRN\",");
        sb.AppendLine();
        sb.AppendLine("    t.\"Street\"                               AS \"Street\",");
        sb.AppendLine("    t.\"Locality\"                             AS \"Locality\",");
        sb.AppendLine("    t.\"Address3\"                             AS \"Address3\",");
        sb.AppendLine("    t.\"Town\"                                 AS \"Town\",");
        sb.AppendLine("    t.\"County (name)\"                        AS \"County\",");
        sb.AppendLine("    t.\"Postcode\"                             AS \"Postcode\",");
        sb.AppendLine();
        sb.AppendLine("    t.\"HeadTitle (name)\"                     AS \"HeadTitle\",");
        sb.AppendLine("    t.\"HeadFirstName\"                        AS \"HeadFirstName\",");
        sb.AppendLine("    t.\"HeadLastName\"                         AS \"HeadLastName\",");
        sb.AppendLine("    t.\"HeadPreferredJobTitle\"                AS \"HeadPreferredJobTitle\",");
        sb.AppendLine();
        sb.AppendLine("    t.\"UrbanRural (code)\"                    AS \"UrbanRuralId\",");
        sb.AppendLine("    t.\"UrbanRural (name)\"                    AS \"UrbanRuralName\",");
        sb.AppendLine();
        sb.AppendLine("    t.\"SchoolWebsite\"                        AS \"Website\",");
        sb.AppendLine("    clean_int(t.\"Easting\")                   AS \"Easting\",");
        sb.AppendLine("    clean_int(t.\"Northing\")                  AS \"Northing\"");
        sb.AppendLine($"FROM {rawTable} t;");
        sb.AppendLine();
        sb.AppendLine("CREATE UNIQUE INDEX idx_v_establishment_urn ON v_establishment (\"URN\");");

        return sb.ToString();
    }

    // =====================================================
    // MIRROR VIEWS
    // =====================================================

    private static string GenerateMirrorMaterializedView(string viewName, string? rawTable)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"-- AUTO-GENERATED MIRROR MATERIALIZED VIEW: {viewName}");
        sb.AppendLine();
        sb.AppendLine($"DROP MATERIALIZED VIEW IF EXISTS {viewName};");
        sb.AppendLine();
        sb.AppendLine($"CREATE MATERIALIZED VIEW {viewName} AS");
        sb.AppendLine($"SELECT * FROM {rawTable};");
        return sb.ToString();
    }

    // =====================================================
    // GENERIC MATERIALIZED VIEW (DataMap-driven)
    // =====================================================

    private string GenerateMaterializedView(string viewName, List<DataMapRow> rows, Dictionary<string, string> tableMap)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"-- AUTO-GENERATED MATERIALIZED VIEW: {viewName}");
        sb.AppendLine();
        sb.AppendLine($"DROP MATERIALIZED VIEW IF EXISTS {viewName};");
        sb.AppendLine();
        sb.AppendLine($"CREATE MATERIALIZED VIEW {viewName} AS");
        sb.AppendLine("WITH");

        var groups = rows.GroupBy(r => (r.FileName ?? "").Trim().TrimStart('\uFEFF')).ToList();

        for (int i = 0; i < groups.Count; i++)
        {
            var g = groups[i];
            var r0 = g.First();

            var fileKey = (g.Key ?? "").Trim().TrimStart('\uFEFF');

            if (!TryResolveRawTable(tableMap, fileKey, out var rawTable))
                throw new InvalidOperationException($"Missing table mapping for '{g.Key}'");

            sb.AppendLine($"src_{i + 1} AS (");
            sb.AppendLine("    SELECT");
            sb.AppendLine($"        t.\"{r0.RecordFilterBy}\" AS \"Id\",");

            var props = g
                .Where(r => !string.IsNullOrWhiteSpace(r.PropertyName))
                .GroupBy(r => r.PropertyName!)
                .Select(BuildAggregatedExpression)
                .ToList();

            for (int j = 0; j < props.Count; j++)
                sb.AppendLine($"        {props[j]}{(j == props.Count - 1 ? "" : ",")}");

            sb.AppendLine($"    FROM {rawTable} t");
            sb.AppendLine($"    GROUP BY t.\"{r0.RecordFilterBy}\"");
            sb.AppendLine(")");
            sb.AppendLine(i == groups.Count - 1 ? "," : ",");
        }

        // all_ids
        sb.AppendLine("all_ids AS (");
        for (int i = 0; i < groups.Count; i++)
        {
            var union = i == 0 ? "    " : "    UNION ";
            sb.AppendLine($"{union}SELECT \"Id\" FROM src_{i + 1}");
        }
        sb.AppendLine(")");
        sb.AppendLine();
        sb.AppendLine("SELECT");

        // Always include Id from all_ids
        sb.AppendLine("    a.\"Id\" AS \"Id\",");

        // If this is an establishment-level fact view, include LA/Region dims
        var includeEstablishmentDims =
            viewName.StartsWith("v_establishment_", StringComparison.OrdinalIgnoreCase);

        if (includeEstablishmentDims)
        {
            sb.AppendLine("    e.\"LAId\" AS \"LAId\",");
            sb.AppendLine("    e.\"LAName\" AS \"LAName\",");
            sb.AppendLine("    e.\"RegionId\" AS \"RegionId\",");
            sb.AppendLine("    e.\"RegionName\" AS \"RegionName\",");
        }

        // Build property -> sources lookup
        var propertySources = groups
            .SelectMany((g, idx) => g.Select(r => new { r.PropertyName, Source = idx + 1 }))
            .Where(x => !string.IsNullOrWhiteSpace(x.PropertyName))
            .GroupBy(x => x.PropertyName!)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Source).Distinct().OrderBy(x => x).ToList()
            );

        var orderedProps = propertySources.Keys.OrderBy(p => p).ToList();

        for (int i = 0; i < orderedProps.Count; i++)
        {
            var prop = orderedProps[i];
            var sources = propertySources[prop];

            // note: trailing comma handled based on last column overall
            var isLast = i == orderedProps.Count - 1;
            var comma = isLast ? "" : ",";

            var expr = sources.Count == 1
                ? $"src_{sources[0]}.\"{prop}\""
                : $"COALESCE({string.Join(", ", sources.Select(s => $"src_{s}.\"{prop}\""))})";

            sb.AppendLine($"    {expr} AS \"{prop}\"{comma}");
        }

        sb.AppendLine("FROM all_ids a");

        for (int i = 0; i < groups.Count; i++)
            sb.AppendLine($"LEFT JOIN src_{i + 1} ON src_{i + 1}.\"Id\" = a.\"Id\"");

        if (includeEstablishmentDims)
            sb.AppendLine("LEFT JOIN v_establishment e ON e.\"URN\" = a.\"Id\"");

        sb.AppendLine(";");

        return sb.ToString();
    }

    // =====================================================
    // RAW_SOURCES + RESOLUTION
    // =====================================================

    private static List<RawSource> LoadRawSources()
    {
        var path = RawSourcesCandidates.FirstOrDefault(File.Exists);
        if (path == null)
            throw new FileNotFoundException(
                $"raw_sources.json not found. Tried: {string.Join(", ", RawSourcesCandidates)}");

        var json = File.ReadAllText(path);
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var sources = JsonSerializer.Deserialize<List<RawSource>>(json, opts) ?? new List<RawSource>();

        return sources.Where(s => !string.IsNullOrWhiteSpace(s.FileName)).ToList();
    }

    private static bool TryResolveManagedDatasetKey(
        List<RawSource> sources,
        Dictionary<string, string> tableMap,
        string sourceOrg,
        string type,
        string subtype,
        string year,
        out string datasetKey)
    {
        datasetKey = "";

        static string Norm(string? s) =>
            Regex.Replace((s ?? "").Trim(), "\\s+", " ").ToLowerInvariant();

        bool Matches(RawSource s) =>
            Norm(s.SourceOrg) == Norm(sourceOrg) &&
            Norm(s.Type) == Norm(type) &&
            Norm(s.Subtype) == Norm(subtype);

        // 1) Exact match org/type/subtype/year
        var src = sources.FirstOrDefault(s => Matches(s) && Norm(s.Year) == Norm(year));

        // 2) If asking for Current but it isn't present, pick best available for same org/type/subtype
        if (src == null && Norm(year) == "current")
        {
            var candidates = sources.Where(Matches).ToList();
            if (candidates.Count == 0) return false;

            int YearScore(string y)
            {
                var ny = Norm(y);
                if (ny == "current") return int.MaxValue;

                var digits = new string(ny.Where(char.IsDigit).ToArray());
                if (digits.Length == 0) return 0;

                digits = digits.Length > 8 ? digits[..8] : digits;
                return int.TryParse(digits, out var v) ? v : 0;
            }

            src = candidates.OrderByDescending(s => YearScore(s.Year)).FirstOrDefault();
        }

        if (src == null || string.IsNullOrWhiteSpace(src.FileName))
            return false;

        var pattern = src.FileName.Trim();

        // 3) Dated datasets (GIAS): contains YYYYmmDD => pick latest matching key by date
        if (pattern.Contains("YYYYmmDD", StringComparison.OrdinalIgnoreCase))
        {
            var regex = BuildYyyyMmDdRegex(pattern);

            var candidates = tableMap.Keys
                .Select(k => k.Trim().TrimStart('\uFEFF'))
                .Select(k => new { Key = k, Match = regex.Match(k) })
                .Where(x => x.Match.Success)
                .Select(x => new { x.Key, Date = x.Match.Groups["date"].Value })
                .Where(x => x.Date.Length == 8)
                .OrderByDescending(x => x.Date, StringComparer.Ordinal)
                .ToList();

            if (candidates.Count == 0)
                return false;

            datasetKey = candidates[0].Key;
            return true;
        }

        // 4) Non-dated (EES): exact key, or key with managed version suffix (_v1.0 etc)
        var exact = pattern.Trim();

        var exactMatch = tableMap.Keys
            .Select(k => k.Trim().TrimStart('\uFEFF'))
            .FirstOrDefault(k => k.Equals(exact, StringComparison.OrdinalIgnoreCase));

        if (exactMatch != null)
        {
            datasetKey = exactMatch;
            return true;
        }

        var baseName = exact;

        var versionRegex = new Regex(
            "^" + Regex.Escape(baseName) + "_v(?<ver>[0-9]+\\.[0-9]+(\\.[0-9]+)?)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        var versionCandidates = tableMap.Keys
            .Select(k => k.Trim().TrimStart('\uFEFF'))
            .Select(k => new { Key = k, Match = versionRegex.Match(k) })
            .Where(x => x.Match.Success)
            .Select(x =>
            {
                var verText = x.Match.Groups["ver"].Value;
                Version.TryParse(verText, out var ver);
                return new { x.Key, Version = ver ?? new Version(0, 0) };
            })
            .OrderByDescending(x => x.Version)
            .ToList();

        if (versionCandidates.Count > 0)
        {
            datasetKey = versionCandidates[0].Key;
            return true;
        }

        return false;
    }

    private static Regex BuildYyyyMmDdRegex(string fileNamePattern)
    {
        // Supports exactly one YYYYmmDD placeholder.
        // Example: "edubasealldataYYYYmmDD"
        // Regex:   ^edubasealldata(?<date>\d{8})$
        // Tolerates optional ".csv", ".zip", or ".csv.zip"
        const string token = "YYYYmmDD";

        var idx = fileNamePattern.IndexOf(token, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
            throw new ArgumentException($"Pattern does not contain {token}: '{fileNamePattern}'");

        var prefix = fileNamePattern.Substring(0, idx);
        var suffix = fileNamePattern.Substring(idx + token.Length);

        var rx =
            "^" +
            Regex.Escape(prefix) +
            "(?<date>\\d{8})" +
            Regex.Escape(suffix) +
            "(?:\\.(?:csv|zip)(?:\\.zip)?)?$";

        return new Regex(rx, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    // =====================================================
    // HELPERS
    // =====================================================

    private static bool TryResolveRawTable(
        Dictionary<string, string> tableMap,
        string? datasetKey,
        out string? rawTable)
    {
        rawTable = "";

        if (string.IsNullOrWhiteSpace(datasetKey))
            return false;

        var key = datasetKey.Trim().TrimStart('\uFEFF');

        // 1) Exact
        if (tableMap.TryGetValue(key, out rawTable))
            return true;

        // 2) manual_ canonical fallback
        var manualKey = "manual_" + key;
        if (tableMap.TryGetValue(manualKey, out rawTable))
            return true;

        // 3) Managed version suffix fallback: <base>_v1.0, _v2.1, _v1.0.3 etc (pick highest)
        var versionRegex = new Regex(
            "^" + Regex.Escape(key) + "_v(?<ver>[0-9]+\\.[0-9]+(\\.[0-9]+)?)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        var best = tableMap.Keys
            .Select(k => k.Trim().TrimStart('\uFEFF'))
            .Select(k => new { Key = k, Match = versionRegex.Match(k) })
            .Where(x => x.Match.Success)
            .Select(x =>
            {
                var verText = x.Match.Groups["ver"].Value;
                Version.TryParse(verText, out var ver);
                return new { x.Key, Version = ver ?? new Version(0, 0) };
            })
            .OrderByDescending(x => x.Version)
            .FirstOrDefault();

        if (best != null && tableMap.TryGetValue(best.Key, out rawTable))
            return true;

        return false;
    }

    private static string BuildAggregatedExpression(IEnumerable<DataMapRow> rows)
    {
        var r = rows.First();
        var conditions = new List<string>();
        static string SqlLiteral(string? s) => (s ?? "").Replace("'", "''");

        if (!string.IsNullOrWhiteSpace(r.Filter))
            conditions.Add($"t.\"{r.Filter}\" = '{SqlLiteral(r.FilterValue)}'");
        if (!string.IsNullOrWhiteSpace(r.Filter2))
            conditions.Add($"t.\"{r.Filter2}\" = '{SqlLiteral(r.Filter2Value)}'");
        if (!string.IsNullOrWhiteSpace(r.Filter3))
            conditions.Add($"t.\"{r.Filter3}\" = '{SqlLiteral(r.Filter3Value)}'");

        var whenClause = conditions.Count == 0 ? "TRUE" : string.Join(" AND ", conditions);

        return $"MAX(CASE WHEN {whenClause} THEN {BuildValueExpression(r)} END) AS \"{r.PropertyName}\"";
    }

    private static string BuildValueExpression(DataMapRow r) =>
        r.DataType?.ToLowerInvariant() switch
        {
            "int" => $"clean_int(t.\"{r.Field}\")",
            "percentage" => $"clean_numeric(t.\"{r.Field}\")",
            "numeric" => $"clean_numeric(t.\"{r.Field}\")",
            _ => $"t.\"{r.Field}\""
        };

    private Dictionary<string, string> LoadTableMappings() =>
        File.ReadAllLines(_tableMappingPath)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(','))
            .Where(p => p.Length == 2)
            .ToDictionary(
                p => p[0].Trim().TrimStart('\uFEFF'),
                p => p[1].Trim(),
                StringComparer.OrdinalIgnoreCase
            );

    private static bool IsIgnored(DataMapRow r)
    {
        return string.Equals(
            r.IgnoreMapping?.Trim(),
            "Y",
            StringComparison.OrdinalIgnoreCase);
    }
}
