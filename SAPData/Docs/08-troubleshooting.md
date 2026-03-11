
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

## SAPData project

- 'Missing table mapping or missing data file for xxxxxxx' - usually missing data file
- Check all files have been copied to SourceFiles folder if running locally
- Check all files are copied into BLOB storage container (schooldata) if running in pipeline