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
   dotnet run --project Tests/SAPPub.E2E.TestDataGenerator
   ```
   If `--output` is not specified, the default output directory is `Tests/SAPPub.E2E.Tests/TestData/2526/KS4`.
1. The generated JSON files will be written to the specified output directory.
1. Check in the generated files to the repository.
