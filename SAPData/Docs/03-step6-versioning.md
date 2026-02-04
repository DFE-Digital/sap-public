
# Step 6 - Dataset Versioning and Blob Lifecycle

Step 6 keeps Blob storage clean and ensures downstream steps use only correct current raw inputs.
After each pipeline run, a step6-report artifact is published in GitHub Actions containing dataset-level status and versioning details.

## Responsibilities

- Read SAPData/raw_sources.json
- Determine latest published dataset per source
- Download datasets to a temporary working directory
- Apply naming rules to create canonical blob names
- Upload to Azure Blob Storage
- Delete the previous managed blob (only after upload succeeds)
- Update versions.json in Blob Storage
- Output latest_files mapping for Step 7

## Dataset Types

### GIAS

- Date-based URLs
- London timezone probing
- Secret URL tokens resolved from environment variables
- Optional ZIP extraction

Naming example:

edubasealldataYYYYmmDD -> edubasealldata20260128.csv

### EES

- Versioned REST API
- Latest Published version selected

Naming example:

ks4performance_v1.2.csv

## Blob Lifecycle Rules

- Only latest managed blob is kept
- Old managed blobs deleted after successful upload
- manual_* blobs are never modified

## versions.json Fields

Common:

- latest
- lastSignature
- lastFileName

GIAS:

- lastSuccessDate

EES:

- eesLatestVersion
