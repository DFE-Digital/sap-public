using FluentAssertions;
using SAPData.Models;
using Xunit;

namespace SAPData.Tests.Unit;

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
    public void Generates_establishment_view()
    {
        WriteMapping(("edubasealldata", "raw_edubase_123"));

        var rows = new List<DataMapRow>();

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "03_v_establishment.sql");
        File.Exists(path).Should().BeTrue();

        var sql = File.ReadAllText(path);
        sql.Should().Contain("CREATE MATERIALIZED VIEW v_establishment");
        sql.Should().Contain("FROM raw_edubase_123");
        sql.Should().Contain("idx_v_establishment_urn");
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
        File.Exists(path).Should().BeTrue();

        var sql = File.ReadAllText(path);
        sql.Should().Contain("CREATE MATERIALIZED VIEW v_england_destinations");
        sql.Should().Contain("MAX(CASE WHEN TRUE THEN");
        sql.Should().Contain("AS \"Overall\"");
    }

    [Fact]
    public void Skips_view_when_no_matching_rows()
    {
        WriteMapping(("ks4_dest", "raw_ks4_dest_abc"));

        var rows = new List<DataMapRow>
        {
            // Different range/type → no output
            Row("ks4_dest", "LA", "KS4_Performance", "P8", "p8")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "03_v_england_destinations.sql");
        File.Exists(path).Should().BeFalse();
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

        Action act = () =>
            new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("*Missing table mapping*");
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

        sql.Should().Contain("COALESCE(");
        sql.Should().Contain("src_1.\"Att8\"");
        sql.Should().Contain("src_2.\"Att8\"");
    }

}

