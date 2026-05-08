using Microsoft.AspNetCore.DataProtection;

namespace SAPSec.Web.Setup;

public static class WebApplicationBuilderExtensions
{
    private const string ApplicationName = "SAPPub";
    //private const string StorageConnectionStringKey = "StorageConnectionString";
    //private const string BlobName = "keys.xml";
    //private const string LocalKeysDirectoryName = "SAPPub-Keys";

    /// <summary>
    /// Adds data protection services with environment-specific configuration.
    /// Development: Uses local file system.
    /// Production: Uses Azure Blob Storage with encryption at rest.
    /// Ensures data protection keys are shared across multiple pod instances in Kubernetes.
    /// </summary>
    public static void AddDataProtectionServices(this WebApplicationBuilder builder)
    {
        var dataProtection = builder.Services.AddDataProtection()
            .SetApplicationName(ApplicationName);

        if (builder.Environment.IsDevelopment())
        {
            var localKeysDirectoryName = builder.Configuration["DataProtection:LocalKeysDirectoryName"] ?? "SAPPub-Keys";
            ConfigureLocalDataProtection(dataProtection, localKeysDirectoryName);
            return;
        }

        var storageConnectionString = builder.Configuration["StorageConnectionString"];

        if (string.IsNullOrWhiteSpace(storageConnectionString))
        {
            ConfigureEphemeralDataProtection(dataProtection);
            return;
        }

        var containerName = builder.Configuration["DataProtection:ContainerName"] ?? "keys";
        var blobName = builder.Configuration["DataProtection:BlobName"] ?? "keys.xml";

        ConfigureAzureBlobDataProtection(dataProtection, storageConnectionString, containerName, blobName);
    }

    private static void ConfigureLocalDataProtection(IDataProtectionBuilder dataProtection, string localKeysDirectoryName)
    {
        var localPath = Path.Combine(Path.GetTempPath(), localKeysDirectoryName);
        Directory.CreateDirectory(localPath);

        dataProtection.PersistKeysToFileSystem(new DirectoryInfo(localPath));

        LogDataProtectionConfiguration("Local file system", localPath);
    }

    private static void ConfigureEphemeralDataProtection(IDataProtectionBuilder dataProtection)
    {
        dataProtection.UseEphemeralDataProtectionProvider();

        LogWarning(
            "Using ephemeral data protection keys (temporary until storage is configured)",
            "Keys will be lost on pod restart - reduce to 1 replica to avoid authentication issues"
        );
    }

    private static void ConfigureAzureBlobDataProtection(
        IDataProtectionBuilder dataProtection,
        string connectionString,
        string containerName,
        string blobName)
    {
        try
        {
            dataProtection.PersistKeysToAzureBlobStorage(connectionString, containerName, blobName);
            LogDataProtectionConfiguration(
                "Azure Blob Storage",
                $"{containerName}/{blobName}",
                "Keys encrypted at rest with Azure Storage infrastructure encryption"
            );
        }
        catch (Exception ex)
        {
            LogError("Failed to configure Azure Blob Storage data protection", ex);
            throw new InvalidOperationException(
                $"Data protection setup failed. Ensure storage account is accessible and container '{containerName}' exists.",
                ex
            );
        }
    }

    private static void LogDataProtectionConfiguration(string provider, string location, string? additionalInfo = null)
    {
        Console.WriteLine($"Data Protection: {provider} ({location})");

        if (!string.IsNullOrWhiteSpace(additionalInfo))
        {
            Console.WriteLine($"{additionalInfo}");
        }
    }

    private static void LogWarning(params string[] messages)
    {
        foreach (var message in messages)
        {
            Console.WriteLine($"{message}");
        }
    }

    private static void LogError(string message, Exception ex)
    {
        Console.WriteLine($"{message}");
        Console.WriteLine($"Error: {ex.Message}");

        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner: {ex.InnerException.Message}");
        }
    }
}