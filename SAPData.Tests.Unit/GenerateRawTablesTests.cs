using FluentAssertions;
using System.Text;
using Xunit;

namespace SAPData.Tests.Unit;

public class GenerateRawTablesTests : IDisposable
{
    private readonly string _root;
    private readonly string _input;
    private readonly string _clean;
    private readonly string _sql;

    public GenerateRawTablesTests()
    {
        _root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _input = Path.Combine(_root, "input");
        _clean = Path.Combine(_root, "clean");
        _sql = Path.Combine(_root, "sql");

        Directory.CreateDirectory(_input);
        Directory.CreateDirectory(_clean);
        Directory.CreateDirectory(_sql);
    }

    public void Dispose()
    {
        if (Directory.Exists(_root))
            Directory.Delete(_root, recursive: true);
    }

    // -------------------------------------------------------
    // HELPERS
    // -------------------------------------------------------
    private void WriteCsv(string name, string content)
    {
        File.WriteAllText(
            Path.Combine(_input, name + ".csv"),
            content,
            new UTF8Encoding(false));
    }

    // -------------------------------------------------------
    // TESTS
    // -------------------------------------------------------

    [Fact]
    public void Generates_all_expected_sql_files()
    {
        WriteCsv("test-dataset", "a,b\n1,2");

        new GenerateRawTables(_input, _clean, _sql).Run();

        File.Exists(Path.Combine(_sql, "01_create_raw_tables.sql")).Should().BeTrue();
        File.Exists(Path.Combine(_sql, "02_copy_into_raw.sql")).Should().BeTrue();
        File.Exists(Path.Combine(_sql, "02_copy_into_raw_local.sql")).Should().BeTrue();
        File.Exists(Path.Combine(_sql, "tablemapping.csv")).Should().BeTrue();
    }

    [Fact]
    public void Generates_short_table_names_with_hash()
    {
        WriteCsv(
            "this-is-a-very-very-long-dataset-name-that-would-break-postgres",
            "a\n1");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var mapping = File.ReadAllText(Path.Combine(_sql, "tablemapping.csv"));

        mapping.Should().MatchRegex(@"raw_[a-z0-9_]{1,18}_[a-f0-9]{8}");
    }

    [Fact]
    public void Table_name_is_deterministic()
    {
        WriteCsv("dataset", "a\n1");

        new GenerateRawTables(_input, _clean, _sql).Run();
        var first = File.ReadAllText(Path.Combine(_sql, "tablemapping.csv"));

        Directory.Delete(_sql, true);
        Directory.CreateDirectory(_sql);

        new GenerateRawTables(_input, _clean, _sql).Run();
        var second = File.ReadAllText(Path.Combine(_sql, "tablemapping.csv"));

        first.Should().Be(second);
    }

    [Fact]
    public void Pads_missing_columns()
    {
        WriteCsv(
            "padtest",
            "a,b,c\n1,2");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var cleaned = File.ReadAllLines(Path.Combine(_clean, "padtest.clean.csv"));
        cleaned[1].Should().Be("1,2,");
    }

    [Fact]
    public void Truncates_extra_columns()
    {
        WriteCsv(
            "trunctest",
            "a,b\n1,2,3,4");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var cleaned = File.ReadAllLines(Path.Combine(_clean, "trunctest.clean.csv"));
        cleaned[1].Should().Be("1,2");
    }

    [Fact]
    public void Sanitises_column_names()
    {
        WriteCsv(
            "sanitize",
            "LA-code,School-Name\n1,X");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var sql = File.ReadAllText(Path.Combine(_sql, "01_create_raw_tables.sql"));

        sql.Should().Contain("\"LA_code\"");
        sql.Should().Contain("\"School_Name\"");
    }

    [Fact]
    public void Writes_clean_csv_without_bom()
    {
        WriteCsv("bomtest", "a\n1");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var bytes = File.ReadAllBytes(Path.Combine(_clean, "bomtest.clean.csv"));

        bytes.Take(3).Should().NotBeEquivalentTo(new byte[] { 0xEF, 0xBB, 0xBF });
    }
}
