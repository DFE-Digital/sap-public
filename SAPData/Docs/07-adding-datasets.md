
# Adding New Datasets

Three supported ingestion patterns.

## GIAS

- Add entry to SAPData/raw_sources.json
- FileName must contain YYYYmmDD
- Url must be secret token

Example output:
edubasealldata20260128.csv

## EES

- Add DataSetId
- Leave Url blank

Example output:
ks4performance_v1.2.csv

## Manual

Upload directly to Blob:

manual_example.csv

Always downloaded by Step 7.
