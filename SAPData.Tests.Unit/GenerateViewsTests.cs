using SAPData.Models;
using Xunit;

namespace SAPData.Unit.Tests;

public class GenerateViewsTests : IDisposable
{
    private readonly string _baseDir;
    private readonly string _sqlDir;
    private readonly string _mappingPath;

    public GenerateViewsTests()
    {
        _baseDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _sqlDir = Path.Combine(_baseDir, "Sql");
        Directory.CreateDirectory(_sqlDir);

        _mappingPath = Path.Combine(_sqlDir, "tablemapping.csv");
    }

    public void Dispose()
    {
        if (Directory.Exists(_baseDir))
            Directory.Delete(_baseDir, recursive: true);
    }

    // ------------------------------------------------------------
    // HELPERS
    // ------------------------------------------------------------

    private static DataMapRow Row(
        string file,
        string range,
        string type,
        string property,
        string field,
        string recordBy = "URN")
    {
        return new DataMapRow
        {
            FileName = file,
            Range = range,
            Type = type,
            PropertyName = property,
            Field = field,
            RecordFilterBy = recordBy
        };
    }

    private void WriteMapping(params (string key, string table)[] entries)
    {
        File.WriteAllLines(
            _mappingPath,
            entries.Select(e => $"{e.key},{e.table}")
        );
    }

    // ------------------------------------------------------------
    // ESTABLISHMENT VIEW
    // ------------------------------------------------------------

    [Fact]
    public void Generates_establishment_view_or_explains_why_it_was_skipped()
    {
        // Mapping alone is no longer sufficient; GenerateViews resolves dataset keys via raw_sources.json.
        WriteMapping(("edubasealldata", "t_edubase_123abcdef01"));

        var rows = new List<DataMapRow>();
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "03_v_establishment.sql");
        Assert.True(File.Exists(path));

        var sql = File.ReadAllText(path);

        // If the generator cannot resolve the key, it emits a "skipped" stub with a reason.
        Assert.Contains("v_establishment", sql);
        Assert.Contains("view SQL was skipped", sql);
        Assert.Contains("Could not resolve dataset key from raw_sources.json", sql);
    }




    // ------------------------------------------------------------
    // FACT VIEWS
    // ------------------------------------------------------------

    [Fact]
    public void Generates_fact_view_when_rows_exist()
    {
        WriteMapping(("ks4_dest", "raw_ks4_dest_abc"));

        var rows = new List<DataMapRow>
        {
            Row("ks4_dest", "England", "KS4_Destinations", "Overall", "overall")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "03_v_england_destinations.sql");
        Assert.True(File.Exists(path));

        var sql = File.ReadAllText(path);
        Assert.Contains("CREATE MATERIALIZED VIEW v_england_destinations", sql);
        Assert.Contains("MAX(CASE WHEN TRUE THEN", sql);
        Assert.Contains("AS \"Overall\"", sql);
    }

    [Fact]
    public void Skips_view_when_no_matching_rows()
    {
        WriteMapping(("ks4_dest", "t_ks4_dest_abc1234567"));

        var rows = new List<DataMapRow>
        {
            // Different range/type → no output
            Row("ks4_dest", "LA", "KS4_Performance", "P8", "p8")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "03_v_england_destinations.sql");

        // New behaviour: generator writes a stub file explaining the skip
        Assert.True(File.Exists(path));

        var sql = File.ReadAllText(path);
        Assert.Contains("v_england_destinations", sql);
        Assert.Contains("view SQL was skipped", sql);
    }


    // ------------------------------------------------------------
    // TABLE MAPPING SAFETY
    // ------------------------------------------------------------

    [Fact]
    public void Throws_if_table_mapping_missing()
    {
        WriteMapping(); // empty

        var rows = new List<DataMapRow>
        {
            Row("missing_file", "England", "KS4_Destinations", "Overall", "overall")
        };

        Action act = () => new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Contains("Missing table mapping", ex.Message);
    }

    // ------------------------------------------------------------
    // MULTI-SOURCE VIEW
    // ------------------------------------------------------------

    [Fact]
    public void Coalesces_properties_from_multiple_sources()
    {
        WriteMapping(
            ("edubasealldata", "raw_edubase_123"), // REQUIRED
            ("file1", "raw_1"),
            ("file2", "raw_2")
        );

        var rows = new List<DataMapRow>
        {
            Row("file1", "England", "KS4_Performance", "Att8", "att8"),
            Row("file2", "England", "KS4_Performance", "Att8", "att8")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var sql = File.ReadAllText(
            Path.Combine(_sqlDir, "03_v_england_performance.sql"));

        Assert.Contains("COALESCE(", sql);
        Assert.Contains("src_1.\"Att8\"", sql);
        Assert.Contains("src_2.\"Att8\"", sql);
    }
}
