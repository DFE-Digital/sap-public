
# Step 7 - Controlled Blob Downloads

Step 7 ensures the SQL generator only receives canonical inputs.

## Download Rules

Downloads only:

1. All manual_* blobs
2. Latest managed blobs returned by Step 6

Everything else is ignored.

## Destination Folder

SAPData/DataMap/SourceFiles

Folder is cleared before each run.

## Benefits

- Prevents stale data usage
- Keeps generator deterministic
- Makes manual historical ingestion explicit
