
# Adding New Datasets

Three supported ingestion patterns.

## GIAS

- Add entry to SAPData/raw_sources.json
- FileName must contain YYYYmmDD
- Url must be secret token - actual url held in GitHub secrets

Example output:
edubasealldata20260128.csv

## EES

- Add DataSetId to SAPData/raw_sources.json
- Leave Url blank

Example output:
202425_performance_tables_schools_provisional_v1.0.1.csv

## Manual

Upload directly to Blob:

manual_example.csv

Always downloaded by Step 7.
