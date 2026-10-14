
# Troubleshooting

## GIAS file not found

Causes:

- File not yet published
- Wrong secret URL template
- Missing {fileDatePostfix}

## EES no published versions

Check dataset versions endpoint.

## SQL execution failures

Common causes:

- Column changes
- DataMap mismatch - check filename in map match filenames in blob (minus 'manual_')
- View dependency order

## Skipped view does not exist

`ERROR: Skipped view v_xxx does not exist in schema public, but it required for selective rebuild`

The run is in selective mode and a view it chose not to regenerate is missing. 
On an empty or restored database, run once with `REBUILD_ALL_RAW_TABLES=true`. Otherwise, see Selective Rebuild.

## SAPData project

- 'Missing table mapping or missing data file for xxxxxxx' - usually missing data file
- Check all files have been copied to SourceFiles folder if running locally
- Check all files are copied into BLOB storage container (specified in the github secret 'AZURE_STORAGE_CONTAINER') if running in pipeline