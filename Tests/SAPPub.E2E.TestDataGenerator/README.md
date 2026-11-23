# TestDataGenerator project

## Introduction

Use this project to regenerate test data files for the E2E tests after any source csv files have been updated.

It reads the source csv files from the SAPData/SourceFiles folder and generates
the JSON test data files.

## Instructions

1. Build the project.
1. Ensure your local SourceFiles folder contains the latest source csv files from the SAPData repository.
1. Run the TestDataGenerator with the appropriate arguments:
   ```bash
   dotnet run --project Tests/SAPPub.E2E.TestDataGenerator -- --key-stage=ks4
   ```
   - `--key-stage` is required. The supported values are: `ks4`. The tool exits with an error if it is missing or unsupported.
   - `--output` is optional. If it is not specified, the default output directory for `ks4` is `Tests/SAPPub.E2E.Tests/TestData/2526/KS4`.
   - `--repo-root` is optional. If it is not specified, the repository root is found by looking for `SAPPub.sln`.
1. The generated JSON files will be written to the specified output directory.
1. Check in the generated files to the repository.

## Adding a new key stage

1. Add a folder for the key stage (for example `KS2`) containing its csv mappings, filter specifications and a generator that implements `ITestDataGenerator`.
1. Register it in `KeyStages.cs` with its name, default output path and a factory for the generator.
1. Add the new value to the list of supported values above.
