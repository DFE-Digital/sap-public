using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit.Abstractions;

namespace SAPPub.Tests.UI.Infrastructure;

[Collection("Playwright Tests")]
public class BasePageTest : PageTest
{
    private readonly string _testName;

    public BasePageTest(ITestOutputHelper output)
        : base()
    {
        // Extract fully qualified test name from xUnit output helper
        _testName = ExtractTestName(output);
    }

    private string ExtractTestName(ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetProperty("TestMember")?.GetValue(output);

        if (testMember == null)
            return $"UnknownTest_{Guid.NewGuid()}";

        var testDisplayName = testMember.GetType().GetProperty("DisplayName")?.GetValue(testMember) as string;

        // Clean invalid filename chars
        foreach (var c in Path.GetInvalidFileNameChars())
            testDisplayName = testDisplayName?.Replace(c, '_');

        return testDisplayName ?? $"UnknownTest_{Guid.NewGuid()}";
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            RecordVideoDir = "SAPPub.Tests.UI/test-artifacts/videos",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        };
    }

    public override async Task InitializeAsync()
    {
        Directory.CreateDirectory("SAPPub.Tests.UI/test-a
