using System.Reflection;
using Xunit;

namespace SAPData.Unit.Tests;

public class ProgramPathDiscoveryTests
{
    private const string ProjectFileName = "SAPData.csproj";

    [Fact]
    public void Returns_current_directory_when_csproj_is_in_current_directory()
    {
        using var tmp = new TempDir();
        var sapDataDir = Path.Combine(tmp.Path, "SAPData");
        Directory.CreateDirectory(sapDataDir);

        File.WriteAllText(Path.Combine(sapDataDir, ProjectFileName), "<Project />");

        var originalCwd = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(sapDataDir);

            var result = InvokeFindProjectDirectoryDownwards(ProjectFileName);

            Assert.Equal(Normalize(sapDataDir), Normalize(result));
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCwd);
        }
    }

    [Fact]
    public void Finds_csproj_by_searching_downwards_from_repo_root()
    {
        using var tmp = new TempDir();
        var repoRoot = tmp.Path;

        var sapDataDir = Path.Combine(repoRoot, "SAPData");
        Directory.CreateDirectory(sapDataDir);
        File.WriteAllText(Path.Combine(sapDataDir, ProjectFileName), "<Project />");

        var originalCwd = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(repoRoot);

            var result = InvokeFindProjectDirectoryDownwards(ProjectFileName);

            Assert.Equal(Normalize(sapDataDir), Normalize(result));
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCwd);
        }
    }

    [Fact]
    public void Falls_back_from_bin_folder_and_finds_csproj_from_project_root_candidate()
    {
        using var tmp = new TempDir();
        var repoRoot = tmp.Path;

        var sapDataDir = Path.Combine(repoRoot, "SAPData");
        Directory.CreateDirectory(sapDataDir);
        File.WriteAllText(Path.Combine(sapDataDir, ProjectFileName), "<Project />");

        // Simulate local execution directory:
        // .../SAPData/bin/Debug/net8.0
        var cwd = Path.Combine(sapDataDir, "bin", "Debug", "net8.0");
        Directory.CreateDirectory(cwd);

        var originalCwd = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(cwd);

            var result = InvokeFindProjectDirectoryDownwards(ProjectFileName);

            Assert.Equal(Normalize(sapDataDir), Normalize(result));
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCwd);
        }
    }

    [Fact]
    public void Prefers_csproj_under_folder_named_SAPData_when_multiple_matches_exist()
    {
        using var tmp = new TempDir();
        var repoRoot = tmp.Path;

        var otherDir = Path.Combine(repoRoot, "OtherProject");
        Directory.CreateDirectory(otherDir);
        File.WriteAllText(Path.Combine(otherDir, ProjectFileName), "<Project />");

        var sapDataDir = Path.Combine(repoRoot, "SAPData");
        Directory.CreateDirectory(sapDataDir);
        File.WriteAllText(Path.Combine(sapDataDir, ProjectFileName), "<Project />");

        var originalCwd = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(repoRoot);

            var result = InvokeFindProjectDirectoryDownwards(ProjectFileName);

            Assert.Equal(Normalize(sapDataDir), Normalize(result));
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCwd);
        }
    }

    [Fact]
    public void Throws_when_no_csproj_found_even_after_bin_fallback()
    {
        using var tmp = new TempDir();

        var originalCwd = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(tmp.Path);

            var ex = Assert.Throws<TargetInvocationException>(() =>
                InvokeFindProjectDirectoryDownwards(ProjectFileName));

            // Reflection wraps exceptions; unwrap the real one:
            var inner = ex.InnerException;
            Assert.NotNull(inner);
            Assert.IsType<DirectoryNotFoundException>(inner);

            Assert.Contains(ProjectFileName, inner!.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCwd);
        }
    }

    // ------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------

    private static string InvokeFindProjectDirectoryDownwards(string projectFileName)
    {
        // Load the SAPData assembly (must be referenced by the test project)
        var asm = LoadTargetAssembly();

        // Program is internal, so we locate it by name
        var programType = asm.GetType("SAPData.Program", throwOnError: true)!;

        var method = programType.GetMethod(
            "FindProjectDirectoryDownwards",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);

        var result = method!.Invoke(null, new object[] { projectFileName });
        Assert.NotNull(result);

        return (string)result!;
    }

    private static Assembly LoadTargetAssembly()
    {
        // If your assembly name differs, change "SAPData" to match.
        // This will work when your tests reference the SAPData project.
        try
        {
            return Assembly.Load("SAPData");
        }
        catch
        {
            // Fallback: attempt to find an already loaded assembly by simple name
            var loaded = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => string.Equals(a.GetName().Name, "SAPData", StringComparison.OrdinalIgnoreCase));

            if (loaded != null)
                return loaded;

            throw;
        }
    }

    private static string Normalize(string path)
        => Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    private sealed class TempDir : IDisposable
    {
        public string Path { get; }

        public TempDir()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "sapdata-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(Path, recursive: true);
            }
            catch
            {
                // ignore cleanup failures on CI
            }
        }
    }
}

