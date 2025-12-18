using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class GenerateStagingTables
{
    private readonly string _sqlDir;

    public GenerateStagingTables(string sqlDir)
    {
        _sqlDir = sqlDir;
    }

    public void Run()
    {
        string templatePath = Path.Combine(_sqlDir, "03_stage_tables_template.sql");
        string mappingPath = Path.Combine(_sqlDir, "tablemapping.csv");
        string outputPath = Path.Combine(_sqlDir, "03_stage_tables.sql");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Missing template file: {templatePath}");

        if (!File.Exists(mappingPath))
            throw new FileNotFoundException($"Missing tablemapping.csv: {mappingPath}");

        var mappings = LoadMappings(mappingPath);
        var datasetMap = DetectDatasets(mappings);

        // -----------------------------------------------------------------
        // 1. Load base template
        // -----------------------------------------------------------------
        string template = File.ReadAllText(templatePath, Encoding.UTF8);

        // -----------------------------------------------------------------
        // 2. Replace placeholders in template
        // -----------------------------------------------------------------
        foreach (var kvp in datasetMap)
        {
            template = template.Replace("{{" + kvp.Key + "}}", kvp.Value);
        }

        // -----------------------------------------------------------------
        // 3. Identify which mappings were used in the template
        // -----------------------------------------------------------------
        HashSet<string> usedPlaceholders = new HashSet<string>(datasetMap.Keys);

        // -----------------------------------------------------------------
        // 4. Identify unused mappings → auto-generate staging
        // -----------------------------------------------------------------
        var allShortnames = mappings.Select(m => m.table).ToList();
        var unused = allShortnames.Where(x => !usedPlaceholders.Contains(x)).ToList();

        var autoStaging = new StringBuilder();
        autoStaging.AppendLine();
        autoStaging.AppendLine("-- ===================================================================");
        autoStaging.AppendLine("-- AUTO-GENERATED STAGING TABLES FOR UNREFERENCED DATASETS");
        autoStaging.AppendLine("-- ===================================================================");
        autoStaging.AppendLine();

        foreach (var shortname in unused)
        {
            string rawTable = mappings.First(m => m.table == shortname).table;

            autoStaging.AppendLine($"-- Auto staging for {shortname}");
            autoStaging.AppendLine($"DROP TABLE IF EXISTS stg_{shortname};");
            autoStaging.AppendLine($"CREATE TABLE stg_{shortname} AS SELECT * FROM {rawTable};");
            autoStaging.AppendLine();
        }

        // -----------------------------------------------------------------
        // 5. Append auto staging to the final output
        // -----------------------------------------------------------------
        string finalOutput = template + "\n\n" + autoStaging.ToString();

        // -----------------------------------------------------------------
        // 6. Sanity check: No unresolved placeholders should remain
        // -----------------------------------------------------------------
        var unresolved = FindUnresolvedPlaceholders(finalOutput);
        if (unresolved.Count > 0)
        {
            throw new InvalidOperationException(
                "Unresolved placeholders found: " + string.Join(", ", unresolved));
        }

        // -----------------------------------------------------------------
        // 7. Write file
        // -----------------------------------------------------------------
        File.WriteAllText(outputPath, finalOutput, Encoding.UTF8);
        Console.WriteLine("Generated staging SQL with auto-generated tables for unused datasets.");
    }

    // ---------------------------------------------------------------
    // Load table short-name → raw table name
    // ---------------------------------------------------------------
    private List<(string key, string table)> LoadMappings(string path)
    {
        var list = new List<(string key, string table)>();

        foreach (var line in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            if (parts.Length < 2) continue;
            list.Add((parts[0].Trim(), parts[1].Trim()));
        }

        return list;
    }

    // ---------------------------------------------------------------
    // Required mappings for special templates only
    // ---------------------------------------------------------------
    private Dictionary<string, string> DetectDatasets(List<(string key, string table)> map)
    {
        var dict = map.ToDictionary(x => x.key, x => x.table);

        Dictionary<string, string> required = new Dictionary<string, string>
        {
            { "GIAS", Require(dict, "GIAS", "GIAS dataset") },

            { "KS4_DEST_INST_2021", Require(dict, "ks4_dm_ud_202122_inst_rev", "KS4 Dest Inst 2021/22") },
            { "KS4_DEST_INST_2022", Require(dict, "ks4_dm_ud_202223_inst_rev", "KS4 Dest Inst 2022/23") },

            { "KS4_DEST_LA_2021", Require(dict, "ks4_dm_ud_202122_la_rev", "KS4 Dest LA 2021/22") },
            { "KS4_DEST_LA_2022", Require(dict, "ks4_dm_ud_202223_la_rev", "KS4 Dest LA 2022/23") },

            { "KS4_DEST_ENG_2021", Require(dict, "ks4_dm_ud_202122_nat_rev", "KS4 Dest England 2021/22") },
            { "KS4_DEST_ENG_2022", Require(dict, "ks4_dm_ud_202223_nat_rev", "KS4 Dest England 2022/23") },

            { "KS4_PERF_INST_2022", Require(dict, "2022-2023_england_ks4final", "KS4 Performance Inst 2022/23") },
            { "KS4_PERF_INST_2023", Require(dict, "202324_performance_tables_schools_final", "KS4 Performance Inst 2023/24") },
            { "KS4_PERF_INST_2024", Require(dict, "202425_performance_tables_schools_provisional", "KS4 Performance Inst 2024/25") },

            { "KS4_PERF_LA", Require(dict, "ees_ks4_la_202223", "KS4 Performance LA 2022/23") },
            { "KS4_PERF_ENG", Require(dict, "ees_ks4_nat_202223", "KS4 Performance Eng 2022/23") },
        };

        return required;
    }

    // ---------------------------------------------------------------
    private string Require(Dictionary<string, string> dict, string key, string description)
    {
        if (!dict.TryGetValue(key, out string? value))
            throw new InvalidOperationException($"Missing mapping for dataset: {description}.");
        return value;
    }

    // ---------------------------------------------------------------
    private List<string> FindUnresolvedPlaceholders(string sql)
    {
        return Regex.Matches(sql, @"\{\{(.*?)\}\}")
                    .Select(m => m.Groups[1].Value)
                    .Distinct()
                    .ToList();
    }
}
